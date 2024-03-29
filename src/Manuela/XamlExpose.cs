// Note #ABOUT-XAML-NS
// should we create a custom ns schema for this?
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0
// the problem is:
// https://learn.microsoft.com/en-us/dotnet/maui/xaml/namespaces/custom-namespace-schemas?view=net-maui-8.0#consume-a-custom-namespace-schema
// is that really necessary?

// as a workaround we do it the old way... expose everything in this namespace

namespace Manuela;

public class AppPage : Controls.AppPage { }
public class AppBody : Controls.AppBody { }

public class SetExtension : Styling.SetExtension { }
public class StylesCollection : Styling.StylesCollection { }

public class TransitionsCollection : Transitions.TransitionsCollection { }
public class Transition : Transitions.Transition { }


public class Checked : Styling.ConditionalStyles.Checked { }
public class Default : Styling.ConditionalStyles.Default { }
public class Disabled : Styling.ConditionalStyles.Disabled { }
public class Focused : Styling.ConditionalStyles.Focused { }
public class Hovered : Styling.ConditionalStyles.Hovered { }
public class Pressed : Styling.ConditionalStyles.Pressed { }
public class Selected : Styling.ConditionalStyles.Selected { }
public class Unchecked : Styling.ConditionalStyles.Unchecked { }

public class OnXsScreen : Styling.ConditionalStyles.Screen.OnXsScreen { }
public class OnSmallScreen : Styling.ConditionalStyles.Screen.OnSmallScreen { }
public class OnMediumScreen : Styling.ConditionalStyles.Screen.OnMediumScreen { }
public class OnLargeScreen : Styling.ConditionalStyles.Screen.OnLargeScreen { }
public class OnXlScreen : Styling.ConditionalStyles.Screen.OnXlScreen { }
public class OnXxlScreen : Styling.ConditionalStyles.Screen.OnXxlScreen { }

public class OnDesktop : Styling.ConditionalStyles.Device.OnDesktop { }
public class OnPhone : Styling.ConditionalStyles.Device.OnPhone { }
public class OnTablet : Styling.ConditionalStyles.Device.OnTablet { }
public class OnTV : Styling.ConditionalStyles.Device.OnTV { }
public class OnWatch : Styling.ConditionalStyles.Device.OnWatch { }

public class OnAndroid : Styling.ConditionalStyles.Platform.OnPlatformAndroid { }
public class OnIOS : Styling.ConditionalStyles.Platform.OnPlatformIOS { }
public class OnMacOS : Styling.ConditionalStyles.Platform.OnPlatformMacOS { }
public class OnTizen : Styling.ConditionalStyles.Platform.OnPlatformTizen { }
public class OnWatchOS : Styling.ConditionalStyles.Platform.OnPlatformWatchOS { }
public class OnWindows : Styling.ConditionalStyles.Platform.OnPlatformWindows { }

public class ManuelaTappedGesture : Behaviors.ManuelaTappedGesture { }
