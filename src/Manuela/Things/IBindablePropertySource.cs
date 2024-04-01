namespace Manuela.Things;

public interface IBindablePropertySource
{
    BindableProperty? Get(BindableObject bindable);
}
