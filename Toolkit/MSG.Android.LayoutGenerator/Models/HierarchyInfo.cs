using Microsoft.CodeAnalysis;
using MSG.Android.LayoutGenerator.Extensions;
using MSG.Android.LayoutGenerator.Helpers;

using static Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle;


namespace MSG.Android.LayoutGenerator.Models;


internal sealed record HierarchyInfo(string FilenameHint, string MetadataName, string Namespace, EquatableArray<TypeInfo> Hierarchy)
{
    public static HierarchyInfo From(INamedTypeSymbol typeSymbol)
    {
        using ImmutableArrayBuilder<TypeInfo> hierarchy = ImmutableArrayBuilder<TypeInfo>.Rent();

        for (INamedTypeSymbol? parent = typeSymbol;
             parent is not null;
             parent = parent.ContainingType)
        {
            hierarchy.Add(new TypeInfo(
                parent.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                parent.TypeKind,
                parent.IsRecord));
        }

        return new(
            typeSymbol.GetFullyQualifiedMetadataName(),
            typeSymbol.MetadataName,
            typeSymbol.ContainingNamespace.ToDisplayString(new(typeQualificationStyle: NameAndContainingTypesAndNamespaces)),
            hierarchy.ToImmutable());
    }
}
