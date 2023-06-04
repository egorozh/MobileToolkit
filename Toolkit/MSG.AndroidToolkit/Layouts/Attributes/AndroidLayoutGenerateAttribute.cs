namespace MSG.AndroidToolkit.Layouts.Attributes;


[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class AndroidLayoutGenerateAttribute : Attribute
{
    public string? LayoutResource { get; init; }
    
    
    public string? Source { get; init; }
}