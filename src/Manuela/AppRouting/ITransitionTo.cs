namespace Manuela.AppRouting;

public interface ITransitionSource : IView
{
    View OverlapElement { get; }
}
