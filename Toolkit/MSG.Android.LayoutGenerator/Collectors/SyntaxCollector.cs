using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.Extensions;


namespace MSG.Android.LayoutGenerator.Collectors;


internal static class SyntaxCollector
{
    public static bool IsLayoutGenerateAttributeNote(SyntaxNode syntaxNode)
    {
        return syntaxNode is AttributeSyntax
        {
            Name: IdentifierNameSyntax
            {
                Identifier.Text: "AndroidLayoutGenerate" or "AndroidLayoutGenerateAttribute"
            }
        };
    }

    
    public static LayoutCollectData? GetCollectData(AttributeSyntax attributeSyntax)
    {
        var argument = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault();
        
        if (argument?.Expression is not MemberAccessExpressionSyntax expression)
            return null;

        string layoutName = expression.Name.ToString();
        
        if (string.IsNullOrEmpty(layoutName))
            return null;

        var classSyntax = attributeSyntax.GetParent<ClassDeclarationSyntax>();

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
        
        return new LayoutCollectData(layoutName, classSyntax, sourceName);
    }
    
    
    public record LayoutCollectData(string LayoutName, ClassDeclarationSyntax ClassSyntax, string? SourceName);
}