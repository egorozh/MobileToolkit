using System.Reflection;
using Microsoft.CodeAnalysis;
using SGM.Android.LayoutGenerator.CodeBuilder;
using SGM.Android.LayoutGenerator.Collectors;


namespace SGM.Android.LayoutGenerator;


[Generator]
public class AndroidLayoutControlsFieldGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxCollector());
        
        context.RegisterForPostInitialization(c =>
        {
            c.AddSource("Attributes.g.cs", ReadResourceClassContent("SGM.Android.LayoutGenerator.Resources.Attributes.cs"));
            c.AddSource("ViewExtensions.g.cs", ReadResourceClassContent("SGM.Android.LayoutGenerator.Resources.ViewExtensions.cs"));
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