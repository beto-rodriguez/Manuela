namespace Manuela.Generation;

public struct TemplateParams(
    TemplateType type,
    string @namespace,
    string typeName,
    // forms params
    FormsPropertyParams[]? properties = null,

    // xaml state params
    string? visualElementParamName = null,
    string? notifiersSyntax = null)
{
    public static TemplateParams Empty = new(TemplateType.Form, string.Empty, string.Empty);

    public TemplateType Type = type;

    public string Namespace = @namespace;
    public string TypeName = typeName;

    public FormsPropertyParams[]? Properties = properties;

    public string? VisualElementParamName = visualElementParamName;
    public string? NotifiersSyntax = notifiersSyntax;
}
