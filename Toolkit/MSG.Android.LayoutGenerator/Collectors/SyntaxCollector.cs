using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.Extensions;


namespace MSG.Android.LayoutGenerator.Collectors;


internal static class SyntaxCollector
{
    public static bool IsLayoutGenerateAttributeNote(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
            return false;

        
        foreach (var attributeList in typeDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                if (IsKnownAttribute(attribute))
                {
                    return true;
                }
            }
        }

        return false;
    }

    
    private static bool IsKnownAttribute(AttributeSyntax attribute)
    {
        if (attribute.Name is not IdentifierNameSyntax identifierNameSyntax)
            return false;

        string attributeName = identifierNameSyntax.Identifier.Text;

        return attributeName is "AndroidLayoutGenerate" or "AndroidLayoutGenerateAttribute";
    }


    public static LayoutCollectData? GetCollectData(TypeDeclarationSyntax typeDeclarationSyntax)
    {
        List<AttributeSyntax> attributeSyntaxes = new ();

        foreach (var attributeList in typeDeclarationSyntax.AttributeLists)
        {
            attributeSyntaxes.AddRange(attributeList.Attributes.Where(static attribute => IsKnownAttribute(attribute)));
        }

        var attributeSyntax = attributeSyntaxes.FirstOrDefault();

        var argument = attributeSyntax?.ArgumentList?.Arguments.FirstOrDefault();
        
        if (argument?.Expression is not MemberAccessExpressionSyntax expression)
            return null;

        string layoutName = expression.Name.ToString();
        
        if (string.IsNullOrEmpty(layoutName))
            return null;
        
        string? sourceName = null;

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
                    sourceNameOfExpression.ArgumentList.Arguments.FirstOrDefault() is {} argumentSyntax)
                {
                    sourceName = argumentSyntax.Expression.ToString();
                }
            }
        }
        
        return new LayoutCollectData(layoutName, typeDeclarationSyntax, sourceName);
    }
    
    
    public record LayoutCollectData(string LayoutName, TypeDeclarationSyntax ClassSyntax, string? SourceName);
}