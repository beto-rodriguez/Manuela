namespace Manuela;

public class ManuelaStyle : Dictionary<ManuelaProperty, object?>
{
    public IList<Setter> AsSetters(BindableObject bindable)
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
