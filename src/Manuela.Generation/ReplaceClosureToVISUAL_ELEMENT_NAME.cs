using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Manuela.Generation;

public class ReplaceClosureToVISUAL_ELEMENT_NAME(string originalName) : CSharpSyntaxRewriter
{
    private readonly string _originalName = originalName;

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Identifier.ValueText == _originalName)
        {
            return node.WithIdentifier(SyntaxFactory.Identifier(TriggerTemplate.VISUAL_ELEMENT_NAME));
        }

        return base.VisitIdentifierName(node);
    }
}
