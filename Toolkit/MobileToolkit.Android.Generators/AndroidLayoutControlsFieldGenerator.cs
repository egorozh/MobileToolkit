﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MobileToolkit.Android.Generators.CodeBuilder;
using MobileToolkit.Android.Generators.Extensions;
using MobileToolkit.Android.Generators.Models;


namespace MobileToolkit.Android.Generators;


public sealed record AndroidLayoutInfo(string LayoutResource, string? Source);


[Generator]
public class AndroidLayoutControlsFieldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
         IncrementalValuesProvider<Result<(HierarchyInfo Hierarchy, AndroidLayoutInfo? Info)>> generationInfoWithErrors =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "MobileToolkit.Android.Layouts.Attributes.AndroidLayoutAttribute",
                static (node, _) => node is ClassDeclarationSyntax classDeclaration && classDeclaration.HasOrPotentiallyHasAttributes(),
                (context, token) =>
                {
                    INamedTypeSymbol typeSymbol = (INamedTypeSymbol)context.TargetSymbol;

                    // Gather all generation info, and any diagnostics
                    AndroidLayoutInfo? info = ValidateTargetTypeAndGetInfo(context.Attributes[0]);

                    token.ThrowIfCancellationRequested();

                    // If there are any diagnostics, there's no need to compute the hierarchy info at all, just return them
                    
                    HierarchyInfo hierarchy = HierarchyInfo.From(typeSymbol);

                    token.ThrowIfCancellationRequested();
                    
                    return new Result<(HierarchyInfo, AndroidLayoutInfo?)>((hierarchy, info));
                })
            .Where(static item => item is not null);
         
        // Get the filtered sequence to enable caching
        IncrementalValuesProvider<(HierarchyInfo Hierarchy, AndroidLayoutInfo Info)> generationInfo =
            generationInfoWithErrors.Select(static (item, _) => item.Value)!;
        
        var configProvider = context.AnalyzerConfigOptionsProvider
            .Select(static (p, ctx) => p.GlobalOptions)
            .Select(static (g, ctx) =>
            {
                if (!g.TryGetValue("build_property.projectdir", out string? projectDir))
                    return null;

                return projectDir;
            });
        
        context.RegisterSourceOutput(generationInfo.Combine(configProvider), static (productionContext, data) =>
        {
            (var (hierarchy, info), string? projectDir) = data;

            if (string.IsNullOrEmpty(projectDir))
                return;
            
            AndroidLayoutFieldsCodeBuilder.Generate(productionContext, hierarchy, info, projectDir);
        });
    }

    
    private static AndroidLayoutInfo ValidateTargetTypeAndGetInfo(AttributeData attributeData)
    {
        string? layoutResource = attributeData.GetNamedArgument("LayoutResource", "");
        string? source = attributeData.GetNamedArgument("Source", "");

        AndroidLayoutInfo info = new AndroidLayoutInfo(LayoutResource: layoutResource, Source: source);

        return info;
    }
}