using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.Extensions;


namespace MSG.Android.LayoutGenerator.Collectors;


internal class SyntaxCollector : ISyntaxReceiver
{
    private readonly List<LayoutCollectData> _collects = new();

    
    public IReadOnlyList<LayoutCollectData> Collects => _collects;

    

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax {Identifier.Text: "AndroidLayoutGenerate" or "AndroidLayoutGenerateAttribute"}} attributeSyntax)
            return;

        var argument = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault();
        
        if (argument?.Expression is not MemberAccessExpressionSyntax expression)
            return;

        string layoutName = expression.Name.ToString();
        
        if (string.IsNullOrEmpty(layoutName))
            return;

        var classSyntax = syntaxNode.GetParent<ClassDeclarationSyntax>();

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
        
        _collects.Add(new LayoutCollectData(layoutName, classSyntax, sourceName));
    }

    
    public record LayoutCollectData(string LayoutName, ClassDeclarationSyntax ClassSyntax, string? SourceName);
}