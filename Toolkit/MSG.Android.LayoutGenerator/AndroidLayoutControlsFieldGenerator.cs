using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using MSG.Android.LayoutGenerator.CodeBuilder;
using MSG.Android.LayoutGenerator.Collectors;


namespace MSG.Android.LayoutGenerator;


[Generator]
public class AndroidLayoutControlsFieldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<LayoutCollectData> providerTypes = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => SyntaxCollector.IsLayoutGenerateAttributeNote(node),
                static (syntaxContext, ctx) => SyntaxCollector.GetCollectData((AttributeSyntax)syntaxContext.Node, ctx))
            .Where(static m => m is not null)!;

        var configProvider = context.AnalyzerConfigOptionsProvider
            .Select(static (p, ctx) => p.GlobalOptions)
            .Select(static (g, ctx) =>
            {
                if (!g.TryGetValue("build_property.projectdir", out string? projectDir))
                    return null;
    
                return projectDir;
            });
        
        var provider = providerTypes.Combine(configProvider);
        
        context.RegisterSourceOutput(provider, static (productionContext, inputs) =>
        {
            LayoutCollectData collectData = inputs.Left;
            string projectDir = inputs.Right;

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