using System.Collections.Immutable;
using System.Xml.Linq;
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
            .OfType<IMemberReferenceOperation>()
            .Where(referenceOperation =>
            {
                var isContainedInNpc = referenceOperation.Member.ContainingType?.AllInterfaces
                    .Any(x => SymbolEqualityComparer.Default.Equals(x, s_npcSymbol)) ?? false;

                return isContainedInNpc;
            });

        foreach (var referenceOperation in referenceOperations)
        {
            var varName = referenceOperation.Instance?.Syntax.ToString() ?? "?";

            // Create a new node with a different name
            var newNode = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(varName),
                SyntaxFactory.IdentifierName($"new"));

            // Replace the old node with the new one
            //root = root.ReplaceNode(oldNode.Syntax, newNode);
            var newRoot = conditionBody.ReplaceNode(referenceOperation.Syntax, newNode);
            var a = newRoot.ToString();

            var replacer = new ReplaceWithNotify(varName);
            var newSyntax = replacer.Visit(referenceOperation.Instance?.Syntax);

            map.AddProperty(newSyntax.ToString(), referenceOperation.Member.Name);
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

            //TriggerTemplate.Generate(context, map);
        }
    }
}

public class ReplaceWithNotify(string originalName) : CSharpSyntaxRewriter
{
    private readonly string _originalName = originalName;

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Identifier.ValueText == _originalName)
        {
            return node.WithIdentifier(SyntaxFactory.Identifier($"Notify({node.Identifier.ValueText}, \"Text\", triggers)"));
        }

        return base.VisitIdentifierName(node);
    }
}
