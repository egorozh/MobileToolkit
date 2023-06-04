using Microsoft.CodeAnalysis;
using MobileToolkit.Android.Generators.Extensions;
using static Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle;


namespace MobileToolkit.Android.Generators.Models;


internal sealed record HierarchyInfo(string FilenameHint, string MetadataName, string Namespace)
{
    public static HierarchyInfo From(INamedTypeSymbol typeSymbol) => new(
        typeSymbol.GetFullyQualifiedMetadataName(),
        typeSymbol.MetadataName,
        typeSymbol.ContainingNamespace.ToDisplayString(new(typeQualificationStyle: NameAndContainingTypesAndNamespaces)));
}