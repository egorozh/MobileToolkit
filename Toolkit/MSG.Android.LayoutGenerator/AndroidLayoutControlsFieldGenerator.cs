using System.Reflection;
using Microsoft.CodeAnalysis;
using MSG.Android.LayoutGenerator.CodeBuilder;
using MSG.Android.LayoutGenerator.Collectors;


namespace MSG.Android.LayoutGenerator;


[Generator]
public class AndroidLayoutControlsFieldGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxCollector());
        
        context.RegisterForPostInitialization(c =>
        {
            string? attrOutput = ReadResourceClassContent("MSG.Android.LayoutGenerator.Resources.Attributes.cs");
            
            if (attrOutput is not null)
                c.AddSource("Attributes.g.cs", attrOutput);
            
            string? viewsExtensionsOutput = ReadResourceClassContent("MSG.Android.LayoutGenerator.Resources.ViewExtensions.cs");
            
            if (viewsExtensionsOutput is not null)
                c.AddSource("ViewExtensions.g.cs", viewsExtensionsOutput);
        });
    }

    
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxCollector syntaxCollector)
            return;

        var collectData = syntaxCollector.Collects;
        
        if (collectData.Count == 0)
            return;
        
        if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out string? projectDir))
            return;
        
        if (string.IsNullOrEmpty(projectDir))
            return;
        
        foreach (var data in collectData) 
            AndroidLayoutFieldsCodeBuilder.Generate(context, data, projectDir);
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