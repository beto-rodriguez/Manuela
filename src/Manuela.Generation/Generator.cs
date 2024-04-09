using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Manuela.Generation;

[Generator]
public class Generator : IIncrementalGenerator
{
    private static ISymbol? s_npcSymbol;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var propertyDeclarations = context.SyntaxProvider
           .CreateSyntaxProvider(
               predicate: IsSyntaxTargetForGeneration,
               transform: GetSemanticTargetForGeneration);

        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(propertyDeclarations.Collect()),
            static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken t)
    {
        return node is ClassDeclarationSyntax cds &&
            cds.BaseList?.Types.FirstOrDefault()?.Type is IdentifierNameSyntax ins
            && ins.Identifier.Text == "XamlState";
    }

    private static TriggersMap? GetSemanticTargetForGeneration(
        GeneratorSyntaxContext context, CancellationToken t)
    {
        s_npcSymbol ??= context.SemanticModel.Compilation
            .GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

        var cds = (ClassDeclarationSyntax)context.Node;
        var classSymbol = (ITypeSymbol?)context.SemanticModel.GetDeclaredSymbol(cds);

        var condition = (MethodDeclarationSyntax?)context.Node
            .DescendantNodes()
            .FirstOrDefault(x => x is MethodDeclarationSyntax mds && mds.Identifier.Text == "IsActive");

        var conditionBody = condition?.Body ?? (SyntaxNode?)condition?.ExpressionBody;

        if (condition is null || conditionBody is null || classSymbol is null) return null;

        var paramName = condition.ParameterList.Parameters.FirstOrDefault()?.Identifier.Text
            ?? "v";

        var map = new TriggersMap(
            paramName,
            classSymbol.Name,
            classSymbol.ContainingNamespace.ToDisplayString());

        var referenceOperations = context.SemanticModel.GetOperation(conditionBody)
            .Descendants()
            .OfType<IMemberReferenceOperation>();

        foreach (var referenceOperation in referenceOperations)
        {
            var isContainedInNpc = referenceOperation.Member.ContainingType?.AllInterfaces
                .Any(x => SymbolEqualityComparer.Default.Equals(x, s_npcSymbol)) ?? false;

            var isTrigger = isContainedInNpc && referenceOperation.Member.Kind == SymbolKind.Property;

            if (!isContainedInNpc || referenceOperation.Member.Kind != SymbolKind.Property) continue;

            var varName = referenceOperation.Instance?.Syntax.ToString() ?? "?";
            map.AddProperty(varName, referenceOperation.Member.Name);
        }

        return map;
    }

    private static void Execute(
       Compilation compilation,
       ImmutableArray<TriggersMap?> notifiers,
       SourceProductionContext context)
    {
        if (notifiers.IsDefaultOrEmpty) return;

        foreach (var map in notifiers)
        {
            if (map is null) continue;

            TriggerTemplate.Generate(context, map);
        }
    }
}
