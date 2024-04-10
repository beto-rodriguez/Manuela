namespace Manuela.Generation;

public struct TemplateParams(
    string @namespace, string typeName, string visualElementParamName, string notifiersSyntax)
{
    public static TemplateParams Empty = new(string.Empty, string.Empty, string.Empty, string.Empty);

    public string Namespace = @namespace;
    public string TypeName = typeName;
    public string VisualElementParamName = visualElementParamName;
    public string NotifiersSyntax = notifiersSyntax;
}
