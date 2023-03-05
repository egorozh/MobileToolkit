using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.CodeBuilder;
using MSG.Android.LayoutGenerator.Collectors;


namespace MSG.Android.LayoutGenerator;


[Generator]
public class AndroidLayoutControlsFieldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<AttributeSyntax> providerTypes = context.SyntaxProvider.CreateSyntaxProvider(
            (node, _) => SyntaxCollector.IsLayoutGenerateAttributeNote(node),
            (syntaxContext, _) => (AttributeSyntax)syntaxContext.Node);

        var provider = providerTypes.Combine(context.AnalyzerConfigOptionsProvider);
        
        context.RegisterSourceOutput(provider, (productionContext, inputs) =>
        {
            var collectData = SyntaxCollector.GetCollectData(inputs.Left);
            
            if (collectData is null)
                return;

            var globalOptions = inputs.Right.GlobalOptions;
            
            if (!globalOptions.TryGetValue("build_property.projectdir", out string? projectDir))
                return;
            
            if (string.IsNullOrEmpty(projectDir))
                return;
            
            AndroidLayoutFieldsCodeBuilder.Generate(productionContext, collectData, projectDir);
        });
        
        
        context.RegisterPostInitializationOutput(c =>
        {
            string? attrOutput = ReadResourceClassContent("MSG.Android.LayoutGenerator.Resources.Attributes.cs");
            
            if (attrOutput is not null)
                c.AddSource("Attributes.g.cs", attrOutput);
            
            string? viewsExtensionsOutput = ReadResourceClassContent("MSG.Android.LayoutGenerator.Resources.ViewExtensions.cs");
            
            if (viewsExtensionsOutput is not null)
                c.AddSource("ViewExtensions.g.cs", viewsExtensionsOutput);
        });
    }
    

    private static string? ReadResourceClassContent(string resourceClassName)
    {
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceClassName);

        if (resourceStream != null)
        {
            using var reader = new StreamReader(resourceStream);

            return reader.ReadToEnd();
        }

        return null;
    }
}