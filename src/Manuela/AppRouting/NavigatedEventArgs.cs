namespace Manuela.AppRouting;

public class NavigatedEventArgs(NavigatingEventArgs navigating, View navigatedTo)
    : NavigatingEventArgs(navigating.Sender, navigating.OldRoute, navigating.NewRoute)
{
    public View NavigatedTo { get; } = navigatedTo;
}
