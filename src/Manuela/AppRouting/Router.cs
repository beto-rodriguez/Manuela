// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Manuela.AppRouting;
using Manuela.Things;

namespace Manuela;

/// <summary>
/// Defines the route map for the application.
/// </summary>
public class Router : INotifyPropertyChanged
{
    private Route _activeRoute = Route.Empty;

    private Router()
    { }

    private NavStack<Route> Navigation { get; } = new(20);
    public static Router Current { get; } = new();
    public List<Route> Routes { get; internal set; } = [];
    public Route ActiveRoute { get => _activeRoute; internal set { _activeRoute = value; OnPropertyChanged(); } }

    public event Action<NavigatingEventArgs>? Navigating;
    public event Action<NavigatedEventArgs>? Navigated;

    public event PropertyChangedEventHandler? PropertyChanged;

#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static BindableProperty LinkProperty = BindableProperty.CreateAttached(
        "Link", typeof(string), typeof(Has), null,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var route = (string)newValue;

            if (bindable is not View view) return;

            // on CollectionViews (virtualized) it seems that the same UI element is reused
            // lets clear the gesture recognizers to avoid multiple taps on different links
            // this has an issue... what if the user is using gestures on the view?

            view.GestureRecognizers.Clear();

            Command cmd = route == "../"
                ? new(() => Current.GoBack())
                : route == "."
                    ? new(() => Current.Reload())
                    : new(() => Current.GoTo(route, view));

            view.GestureRecognizers.Add(new TapGestureRecognizer { Command = cmd });
        });
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static string GetLink(BindableObject view)
    {
        return (string)view.GetValue(LinkProperty);
    }

    public static void SetLink(BindableObject view, string value)
    {
        view.SetValue(LinkProperty, value);
    }

    public void GoTo<T>()
    {
        GoTo(typeof(T).Name, null);
    }

    public void GoTo(string route, View? sender)
    {
        Navigation.Stack(ActiveRoute);
        Trace.WriteLine($"Navigating to {route}.");

        var r = GetRoute(route) ?? throw new InvalidOperationException($"The route {route} does not exists.");

        LoadView(r, sender);
    }

    public void GoForward()
    {
        var r = Navigation.GoForward();
        if (r is null || r.IsEmpty) return;

        LoadView(r, null);
    }

    public void GoBack()
    {
        var r = Navigation.GoBack();
        if (r is null || r.IsEmpty) return;

        LoadView(r, null);
    }

    public void Reload()
    {
        LoadView(ActiveRoute, null);
    }

    private Route? GetRoute(string route)
    {
        var routeSegments = route.Split('/');
        var parameters = new Dictionary<string, string>();

        foreach (var r in Routes)
        {
            var rs = r.RouteName.Split('/');

            if (routeSegments.Length != rs.Length) continue;

            var isMatch = true;
            for (var i = 0; i < routeSegments.Length; i++)
            {
                if (rs[i].StartsWith(':'))
                {
                    parameters[rs[i].Substring(1)] = routeSegments[i];
                    continue;
                }

                if (routeSegments[i] != rs[i])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                r.Parameters = parameters;
                return r;
            }
        }

        return null;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadView(Route route, View? sender)
    {
        var navigationArgs = new NavigatingEventArgs(sender, ActiveRoute, route);
        Navigating?.Invoke(navigationArgs);
        ActiveRoute = route;

        var view = ManuelaThings.ServiceProvider?.GetRequiredService(route.ViewType) as ContentView ??
            throw new InvalidOperationException($"The view type {route.ViewType} is not assignable to ContentView type.");

        if (route.ViewModelType is not null)
        {
            var viewModel = ManuelaThings.ServiceProvider?.GetRequiredService(route.ViewModelType);
            view.BindingContext = viewModel;
        }

        AppPage.Current.Body = view;
        Navigated?.Invoke(new(navigationArgs, AppPage.Current.Body!));
    }

    private class NavStack<T>(int capacity)
    {
        private readonly T?[] _items = new T[capacity];
        private int _top = 0;

        public void Stack(T item)
        {
            _items[_top] = item;
            _top = (_top + 1) % _items.Length;
        }

        public T? GoBack()
        {
            _top = (_items.Length + _top - 1) % _items.Length;
            return _items[_top];
        }

        public T? GoForward()
        {
            _top = (_top + 1) % _items.Length;
            return _items[_top];
        }
    }
}
