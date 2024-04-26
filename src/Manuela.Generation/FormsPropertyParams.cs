namespace Manuela.Generation;

public struct FormsPropertyParams(
    string propertyName,
    string propertyType,
    string propertyDisplaySource)
{
    public string Name = propertyName;
    public string Type = propertyType;
    public string DisplaySource = propertyDisplaySource;
}
