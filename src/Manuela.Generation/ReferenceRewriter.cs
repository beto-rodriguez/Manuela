using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Manuela.Generation;

public class ReferenceRewriter(
    Dictionary<MemberAccessExpressionSyntax, MemberAccessExpressionSyntax> memberAccessReplacements,
    Dictionary<MemberBindingExpressionSyntax, MemberBindingExpressionSyntax> memberBindingReplacements)
        : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        if (memberAccessReplacements.TryGetValue(node, out var newNode))
            node = newNode;

        return base.VisitMemberAccessExpression(node);
    }

    public override SyntaxNode? VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
    {
        if (memberBindingReplacements.TryGetValue(node, out var newNode))
            node = newNode;

        return base.VisitMemberBindingExpression(node);
    }
}
