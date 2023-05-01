using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.Extensions;


namespace MSG.Android.LayoutGenerator.Collectors;


internal static class SyntaxCollector
{
    public static bool IsLayoutGenerateAttributeNote(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attributeSyntax)
            return false;

        return IsKnownAttribute(attributeSyntax);
    }
    

    public static LayoutCollectData? GetCollectData(AttributeSyntax attributeSyntax,
        CancellationToken cancellationToken)
    {
        var argument = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault();

        if (argument?.Expression is not MemberAccessExpressionSyntax expression)
            return null;

        string layoutName = expression.Name.ToString();

        if (string.IsNullOrEmpty(layoutName))
            return null;

        string? sourceName = null;

        TypeDeclarationSyntax typeDeclarationSyntax = attributeSyntax.GetParent<TypeDeclarationSyntax>();


        if (attributeSyntax.ArgumentList?.Arguments.Count > 1)
        {
            AttributeArgumentSyntax? sourceArgument = attributeSyntax.ArgumentList?.Arguments[1];

            if (sourceArgument?.Expression is LiteralExpressionSyntax sourceExpression)
            {
                sourceName = sourceExpression.Token.Value?.ToString();
            }
            else if (sourceArgument?.Expression is InvocationExpressionSyntax sourceNameOfExpression)
            {
                if (sourceNameOfExpression.Expression.ToString() == "nameof" &&
                    sourceNameOfExpression.ArgumentList.Arguments.FirstOrDefault() is { } argumentSyntax)
                {
                    sourceName = argumentSyntax.Expression.ToString();
                }
            }
        }

        return new LayoutCollectData(layoutName, typeDeclarationSyntax, sourceName);
    }
    
    
    private static bool IsKnownAttribute(AttributeSyntax attribute)
    {
        if (attribute.Name is not IdentifierNameSyntax identifierNameSyntax)
            return false;

        string attributeName = identifierNameSyntax.Identifier.Text;

        return attributeName is "AndroidLayoutGenerate" or "AndroidLayoutGenerateAttribute";
    }
}