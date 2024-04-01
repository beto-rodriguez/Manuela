namespace Manuela.Things;

public class PropertySource<T>(BindableProperty property) : IBindablePropertySource
{
    public BindableProperty? Get(BindableObject bindable)
    {
        return bindable is T ? property : null;
    }
}
