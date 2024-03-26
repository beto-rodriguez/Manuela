namespace Manuela.Styling;

public class ManuelaSettersDictionary : Dictionary<ManuelaProperty, object?>
{
    public IList<Setter> AsMauiSetters(BindableObject bindable)
    {
        var setters = new List<Setter>();

        foreach (var set in this)
        {
            setters.Add(new Setter
            {
                Property = ManuelaThings.GetBindableProperty(bindable, set.Key),
                Value = ManuelaThings.TryConvert(bindable, set.Key, set.Value)
            });
        }

        return setters;
    }
}
