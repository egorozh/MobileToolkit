using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace MSG.Android.LayoutGenerator.Collectors;


public class LayoutCollectData : IEquatable<LayoutCollectData>
{
    public LayoutCollectData(string layoutName, TypeDeclarationSyntax classSyntax, string? sourceName)
    {
        LayoutName = layoutName;
        ClassSyntax = classSyntax;
        SourceName = sourceName;
    }

        
    public string LayoutName { get; }
    public TypeDeclarationSyntax ClassSyntax { get; }
    public string? SourceName { get; }

        
    public virtual bool Equals(LayoutCollectData? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return LayoutName == other.LayoutName && 
               ClassSyntax.Equals(other.ClassSyntax) &&
               SourceName == other.SourceName;
    }


    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = LayoutName.GetHashCode();
            hashCode = (hashCode * 397) ^ ClassSyntax.GetHashCode();
            hashCode = (hashCode * 397) ^ (SourceName != null ? SourceName.GetHashCode() : 0);
            return hashCode;
        }
    }
}