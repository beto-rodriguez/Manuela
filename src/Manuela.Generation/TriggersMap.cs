namespace Manuela.Generation;

public class TriggersMap(
    string visualElementParameterName,
    string containingTypeName,
    string containingTypeNamespace)
{

    public string VisualElementParameterName { get; } = visualElementParameterName;
    public string ContainingTypeNamespace { get; } = containingTypeNamespace;
    public string ContainingTypeName { get; } = containingTypeName;
    public Dictionary<string, HashSet<string>> PropertiesByNotifier { get; } = [];

    public void AddProperty(string notifierName, string propertyName)
    {
        if (!PropertiesByNotifier.TryGetValue(notifierName, out var properties))
        {
            properties = [];
            PropertiesByNotifier.Add(notifierName, properties);
        }

        _ = properties.Add(propertyName);
    }
}
