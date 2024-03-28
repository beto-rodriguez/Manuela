using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

// experimental generator that makes XAMLa little bit more simple. 
// this generator does nothing special here yet.

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
        if (node is not PropertyDeclarationSyntax cds) return false;

        foreach (var item in cds.ChildNodes())
        {
            if (item is not IdentifierNameSyntax identifierSyntax) continue;
            if (identifierSyntax.Identifier.Text == "XamlCondition") return true;
        }

        return false;
    }

    private static TriggersMap? GetSemanticTargetForGeneration(
        GeneratorSyntaxContext context, CancellationToken t)
    {
        s_npcSymbol ??= context.SemanticModel.Compilation
            .GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

        var propertyDeclaration = (PropertyDeclarationSyntax)context.Node;
        var propertyDeclarationSymbol = context.SemanticModel.GetDeclaredSymbol(propertyDeclaration, t);

        var result = GetXamlConditionExpression((PropertyDeclarationSyntax)context.Node);

        if (result.RootNode is null || propertyDeclarationSymbol is null) return null;

        var map = new TriggersMap(
            propertyDeclarationSymbol.ContainingType.Name,
            propertyDeclarationSymbol.ContainingNamespace.ToDisplayString(),
            propertyDeclarationSymbol.Name);

        var referenceOperations = context.SemanticModel.GetOperation(result.RootNode)
            .Descendants()
            .OfType<IMemberReferenceOperation>();

#if DEBUG
        Console.WriteLine($"{"Member",-30} {"pName",-30} {"Notify",-20} {"kind",-40} {"trigger?"}");
#endif

        foreach (var referenceOperation in referenceOperations)
        {
            var memberName = referenceOperation.Member.Name;
            var parentMemberName = GetParentMemberName(referenceOperation, result.LambdaParameterName);

            var isContainedInNpc = referenceOperation.Member.ContainingType?.AllInterfaces
                .Any(x => SymbolEqualityComparer.Default.Equals(x, s_npcSymbol)) ?? false;

            var isTrigger = isContainedInNpc && referenceOperation.Member.Kind == SymbolKind.Property;

#if DEBUG
            Console.WriteLine(
                $"{referenceOperation.Member.Name,-30} {parentMemberName,-30} {isContainedInNpc,-20} {referenceOperation.Member.Kind,-40} {isTrigger}");
#endif

            if (!isContainedInNpc || referenceOperation.Member.Kind != SymbolKind.Property) continue;

            map.AddProperty(parentMemberName, memberName);
        }

        return map;
    }

    private static void Execute(
       Compilation compilation,
       ImmutableArray<TriggersMap?> notifiers,
       SourceProductionContext context)
    {
        if (notifiers.IsDefaultOrEmpty) return;

        foreach (var mapGroup in notifiers.GroupBy(x => x?.ContainingTypeName ?? "*"))
        {
            if (mapGroup is null || mapGroup.Key == "*") continue;

            TriggerTemplate.Generate(context, mapGroup);
        }
    }

    private static string GetParentMemberName(
        IMemberReferenceOperation referenceOperation, string lambdaParameterName)
    {
        var instanceSyntax = referenceOperation.Instance?.Syntax;

        var replacer = new ReplaceClosureToVISUAL_ELEMENT_NAME(lambdaParameterName);
        var newSyntax = replacer.Visit(instanceSyntax);

        return newSyntax?.ToString() ?? "this";
    }

    private static RootResult GetXamlConditionExpression(PropertyDeclarationSyntax propertyDeclaration)
    {
        // is there a Roselyn api for this?

        SyntaxNode? conditionDefinition = null;

        // arrow expression
        if (propertyDeclaration.ExpressionBody is not null)
        {
            // arrow expressions return a new instance every time they are called
            // we can't populate the triggers of the XamlCondition instance on every call.

            return new RootResult(
                null,
                null,
                "Arrow expressions should not be used as getters of an XamlCondition property.");
        }
        else if (propertyDeclaration.Initializer is not null)
        {
            conditionDefinition = propertyDeclaration.Initializer.Value;
        }
        else
        {
            var getter = propertyDeclaration.AccessorList?.Accessors
                .FirstOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration);

            if (getter?.ExpressionBody is not null)
            {
                // getter with arrow expression
                conditionDefinition = getter.ExpressionBody;
            }
            else
            {
                // getter with block expression
                conditionDefinition = getter?.Body;
            }
        }

        if (conditionDefinition is null)
        {
            return new RootResult(
                null,
                null,
                "Unable to analyze XamlCondition, initialization from constructor isa not implemented yet!");
        }

        SyntaxNode? constructorLambdaArgument = null;
        string? lambdaParameterName = null;

        if (conditionDefinition is BaseObjectCreationExpressionSyntax ocx)
        {
            var t = ocx.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
            constructorLambdaArgument = ocx.ArgumentList?.Arguments.FirstOrDefault()?.Expression as LambdaExpressionSyntax;
            lambdaParameterName = constructorLambdaArgument?.ChildNodes().OfType<ParameterSyntax>().FirstOrDefault()?.Identifier.Text;
        }
        else
        {
            return new RootResult(
                null,
                null,
                "The XamlCondition property must be initialized with a new instance of XamlCondition.");
        }

        return new RootResult(constructorLambdaArgument, lambdaParameterName, null);
    }

    private class RootResult(SyntaxNode? node, string? lambdaParameterName, string? error)
    {
        public string? Error { get; } = error;
        public SyntaxNode? RootNode { get; } = node;
        public string LambdaParameterName { get; } = lambdaParameterName ?? string.Empty;
    }
}
