using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MSG.Android.LayoutGenerator.Collectors;
using MSG.Android.LayoutGenerator.Extensions;


namespace MSG.Android.LayoutGenerator.CodeBuilder;


internal static class AndroidLayoutFieldsCodeBuilder
{
    public static void Generate(GeneratorExecutionContext context, SyntaxCollector.LayoutCollectData data, string projectDir)
    {
        string layoutsDir = Path.Combine(projectDir, "Resources", "layout");

        string layoutPath = Path.Combine(layoutsDir, data.LayoutName + ".xml");

        FileInfo layoutInfo = new(layoutPath);

        if (layoutInfo.Exists)
        {
            GenerateImpl(context, data, layoutPath);
            return;
        }
        
        layoutPath = Path.Combine(layoutsDir, data.LayoutName + ".axml");
        layoutInfo = new FileInfo(layoutPath);
        
        if (layoutInfo.Exists) 
            GenerateImpl(context, data, layoutPath);
    }

    
    private static void GenerateImpl(GeneratorExecutionContext context, SyntaxCollector.LayoutCollectData data, string layoutPath)
    {
        IReadOnlyList<ControlData> controls = GetControlsFromLayout(layoutPath);
        
        if (controls.Count == 0)
            return;
        
        string className = data.ClassSyntax.Identifier.Text;
        
        var namespaceSyntax = data.ClassSyntax.GetParent<FileScopedNamespaceDeclarationSyntax>();

        string namespaceName = namespaceSyntax.Name.ToString();

       
        StringBuilder sb = new();

        sb.AppendLine("using Android.Widget;");
        sb.AppendLine("using Android.Views;");

        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine($"namespace {namespaceName};");
        
        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine($"partial class {className}");
        sb.AppendLine("{");

        foreach (var controlData in controls)
        {
            sb.AppendLine($"    protected {controlData.ClassName} {controlData.Id} {{ get; private set; }}");
            sb.AppendLine();
        }

        sb.AppendLine();

        sb.AppendLine("    protected void InitializeControls()");
        sb.AppendLine("    {");

        string? sourceName = data.SourceName;
        sourceName = string.IsNullOrEmpty(sourceName) ? "this" : sourceName;

        foreach (var controlData in controls)
        {
            sb.AppendLine($"        {controlData.Id} = {sourceName}.FindViewById<{controlData.ClassName}>(Resource.Id.{controlData.Id});");
            sb.AppendLine();
        }
        
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.Append('}');
        
        context.AddSource($"{className}.g.cs", sb.ToString());
    }

    
    private static IReadOnlyList<ControlData> GetControlsFromLayout(string layoutPath)
    {
        XmlDocument xmlDocument = new();
        
        xmlDocument.Load(layoutPath);

        List<ControlData> controls = new();

        foreach (object childNode in xmlDocument.ChildNodes)
        {
            if (childNode is XmlElement xmlElement) 
                ObserveXmlElement(xmlElement, controls);
        }

        return controls;
    }

    
    private static void ObserveXmlElement(XmlElement xmlElement, List<ControlData> controls)
    {
        void AddToControls(XmlElement element)
        {
            foreach (object? a in element.Attributes)
            {
                if (a is not XmlAttribute attr)
                    continue;

                string value = attr.InnerText;
                
                if (attr.LocalName == "id" && value.StartsWith("@+id/"))
                    controls.Add(new ControlData(value.Substring(5), GetClassName(element.Name)));
            }
        }
        
        AddToControls(xmlElement);

        foreach (object childNode in xmlElement.ChildNodes)
        {
            if (childNode is XmlElement childElement) 
                ObserveXmlElement(childElement, controls);
        }
    }

    
    private static string GetClassName(string xmlClassName)
    {
        if (!xmlClassName.Contains('.'))
            return xmlClassName;

        return "View";
    }


    private record ControlData(string Id, string ClassName);
}

