namespace MobileToolkit.Android.Layouts.Attributes;


[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class AndroidLayoutAttribute : Attribute
{
    public string? LayoutResource { get; init; }
    
    
    public string? Source { get; init; }
}