using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
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
    internal static readonly string s_displayAnnotation =
        "System.ComponentModel.DataAnnotations." + nameof(DisplayAttribute);

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
        if (node is ClassDeclarationSyntax cds)
        {
            var inheritsFrom = cds.BaseList?.Types.FirstOrDefault()?.Type;

            // it is tarrget for generation if:
            return

                // 1. inherits from XamlState
                (inheritsFrom is IdentifierNameSyntax ins && ins.Identifier.Text == "XamlState") ||

                // 2. inherits from a class that inherits from Form<T>
                (inheritsFrom is GenericNameSyntax gns && gns.Identifier.Text == "Form");
        }

        return false;
    }

    private static TemplateParams GetSemanticTargetForGeneration(
        GeneratorSyntaxContext context, CancellationToken t)
    {
        var cds = (ClassDeclarationSyntax)context.Node;
        var inheritsFrom = cds.BaseList?.Types.FirstOrDefault()?.Type;

        if (inheritsFrom is IdentifierNameSyntax ins && ins.Identifier.Text == "XamlState")
        {
            return GetXamlStateSemanticTarget(context, t);
        }
        else if (inheritsFrom is GenericNameSyntax gns && gns.Identifier.Text == "Form")
        {
            return GetFormSemanticTarget(context, t);
        }

        return TemplateParams.Empty;
    }

    private static TemplateParams GetFormSemanticTarget(
        GeneratorSyntaxContext context, CancellationToken t)
    {
        var cds = (ClassDeclarationSyntax)context.Node;
        var classSymbol = (ITypeSymbol?)context.SemanticModel.GetDeclaredSymbol(cds);

        if (classSymbol is null) return TemplateParams.Empty;

        var typeProperties = classSymbol.BaseType?
            .TypeArguments
            .FirstOrDefault()?
            .GetMembers()
            .OfType<IPropertySymbol>()
            ?? [];

        var properties = new List<FormsPropertyParams>();

        foreach (var property in typeProperties)
        {
            var displaySource = GetPropertyDisplaySource(property);

            properties.Add(new FormsPropertyParams(
                property.Name,
                property.Type.ToDisplayString(),
                displaySource));
        }

        return new TemplateParams(
            TemplateType.Form,
            classSymbol.ContainingNamespace.ToDisplayString(),
            classSymbol.Name,
            properties: [.. properties]);
    }

    private static TemplateParams GetXamlStateSemanticTarget(
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

        return new TemplateParams(
            TemplateType.XamlState,
            classSymbol.ContainingNamespace.ToDisplayString(),
            classSymbol.Name,
            visualElementParamName: paramName,
            notifiersSyntax: notifiersBody.ToString());
    }

    private static void Execute(
       Compilation compilation,
       ImmutableArray<TemplateParams> templateParamsCollection,
       SourceProductionContext context)
    {
        if (templateParamsCollection.IsDefaultOrEmpty) return;

        foreach (var templateParams in templateParamsCollection)
        {
            switch (templateParams.Type)
            {
                case TemplateType.Form:
                    FormTemplate.Generate(context, templateParams);
                    break;
                case TemplateType.XamlState:
                    XamlStateTemplate.Generate(context, templateParams);
                    break;
                default:
                    throw new InvalidOperationException("Invalid template type");
            }
        }
    }

    public static string GetPropertyDisplaySource(IPropertySymbol property)
    {
        string propertyDisplaySource;

        var displayAttribute = property
            .GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == s_displayAnnotation);

        if (displayAttribute is null)
        {
            // if the display attribute is not present, we use the property name.
            propertyDisplaySource = @$"""{property.Name}""";
        }
        else
        {
            var name = displayAttribute.NamedArguments
                .FirstOrDefault(x => x.Key == nameof(DisplayAttribute.Name)).Value.Value;

            var resourceType = displayAttribute.NamedArguments
                .FirstOrDefault(x => x.Key == nameof(DisplayAttribute.ResourceType)).Value.Value;

            if (resourceType is null)
            {
                // if the ResourceType is null, we use a string literal.
                propertyDisplaySource = @$"""{(name is null ? null : (string)name) ?? property.Name}""";
            }
            else
            {
                // otherwise, we get it from the resource manager.
                var namedTypeSymbol = (INamedTypeSymbol)resourceType;
                propertyDisplaySource = @$"{namedTypeSymbol.ToDisplayString()}.ResourceManager.GetString(""{name}"")";
            }
        }

        return propertyDisplaySource;
    }
}
