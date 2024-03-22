namespace Manuela.Generation;

public class TriggersMap(
    string containingTypeName, string containingTypeNamespace, string propertyName, string lambaParamName)
{
    public string ContainingTypeNamespace { get; } = containingTypeNamespace;
    public string ContainingTypeName { get; } = containingTypeName;
    public string PropertyName { get; } = propertyName;
    public Dictionary<string, PropertyDefinition> PropertiesByNotifier { get; } = [];

    public void AddProperty(string notifierName, string propertyName)
    {
        if (!PropertiesByNotifier.TryGetValue(notifierName, out var properties))
        {
            properties = new PropertyDefinition(notifierName == lambaParamName);
            PropertiesByNotifier.Add(notifierName, properties);
        }

        _ = properties.DependentProperties.Add(propertyName);
    }

    public class PropertyDefinition(bool isLambdaParameter)
    {
        public bool IsLambdaParameter { get; } = isLambdaParameter;
        public HashSet<string> DependentProperties { get; set; } = [];
    }
}
