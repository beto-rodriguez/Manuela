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

    private static TemplateParams GetSemanticTargetForGeneration(
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
        var paramName = condition?.ParameterList.Parameters.FirstOrDefault()?.Identifier.Text;

        if (condition is null || conditionBody is null || classSymbol is null || paramName is null)
            return TemplateParams.Empty;

        var referenceOperations = context.SemanticModel.GetOperation(conditionBody)
            .Descendants()
            .OfType<IMemberReferenceOperation>()
            .Where(referenceOperation =>
            {
                var isContainedInNpc = referenceOperation.Member.ContainingType?.AllInterfaces
                    .Any(x => SymbolEqualityComparer.Default.Equals(x, s_npcSymbol)) ?? false;

                return isContainedInNpc;
            });

        var memberBindingExpression = new Dictionary<MemberBindingExpressionSyntax, MemberBindingExpressionSyntax>();
        var memberAccessExpression = new Dictionary<MemberAccessExpressionSyntax, MemberAccessExpressionSyntax>();

        foreach (var referenceOperation in referenceOperations)
        {
            var varName = referenceOperation.Member.Name.ToString() ?? "?";

            if (referenceOperation.Syntax is MemberBindingExpressionSyntax mbxs)
            {
                // handles normal member access (Object.Property)

                var newBindingExpression = SyntaxFactory.MemberBindingExpression(
                    mbxs.OperatorToken,
                    SyntaxFactory.IdentifierName(
                        @$"Notify(""{mbxs.Name}"", triggers).{mbxs.Name}")); // <- too lazy to build the correct expression, is this enough?

                memberBindingExpression.Add(mbxs, newBindingExpression);
                continue;
            }
            else if (referenceOperation.Syntax is MemberAccessExpressionSyntax maxs)
            {
                // handles null propagation operator (Object?.Property)

                var newAccessExpression = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    maxs.Expression,
                    maxs.OperatorToken,
                    SyntaxFactory.IdentifierName(
                        @$"Notify(""{maxs.Name}"", triggers).{maxs.Name}")); // <- too lazy to build the correct expression, is this enough?

                memberAccessExpression.Add(maxs, newAccessExpression);
                continue;
            }
            else
            {
                // do we need more cases?
                continue;
            }
        }

        var rewriter = new ReferenceRewriter(memberAccessExpression, memberBindingExpression);
        var notifiersBody = rewriter.Visit(conditionBody);

        return new(
            classSymbol.ContainingNamespace.ToDisplayString(),
            classSymbol.Name,
            paramName,
            notifiersBody.ToString());
    }

    private static void Execute(
       Compilation compilation,
       ImmutableArray<TemplateParams> templateParamsCollection,
       SourceProductionContext context)
    {
        if (templateParamsCollection.IsDefaultOrEmpty) return;

        foreach (var templateParams in templateParamsCollection)
            XamlStateTemplate.Generate(context, templateParams);
    }
}
