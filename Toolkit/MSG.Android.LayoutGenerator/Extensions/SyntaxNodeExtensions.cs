using Microsoft.CodeAnalysis;

namespace MSG.Android.LayoutGenerator.Extensions;


internal static class SyntaxNodeExtensions
{
    public static T GetParent<T>(this SyntaxNode node) where T : SyntaxNode
    {
        while (true)
        {
            if (node.Parent is T parent)
                return parent;

            if (node.Parent is null)
                throw new Exception("Parent is null");

            return GetParent<T>(node.Parent);
        }
    }
}