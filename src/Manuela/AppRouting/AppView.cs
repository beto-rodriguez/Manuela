namespace Manuela.AppRouting;

public class AppView<TView, TViewModel>
{
    public Type ViewType { get; } = typeof(TView);
    public Type ViewModelType { get; } = typeof(TViewModel);
}

public class AppView<TView>
{
    public Type ViewType { get; } = typeof(TView);
}
