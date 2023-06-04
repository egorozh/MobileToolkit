using Microsoft.CodeAnalysis;
using MSG.Android.LayoutGenerator.Extensions;

using static Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle;


namespace MSG.Android.LayoutGenerator.Models;


internal sealed record HierarchyInfo(string FilenameHint, string MetadataName, string Namespace)
{
    public static HierarchyInfo From(INamedTypeSymbol typeSymbol) => new(
        typeSymbol.GetFullyQualifiedMetadataName(),
        typeSymbol.MetadataName,
        typeSymbol.ContainingNamespace.ToDisplayString(new(typeQualificationStyle: NameAndContainingTypesAndNamespaces)));
}