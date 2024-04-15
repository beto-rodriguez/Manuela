 // The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States.Platform;

namespace Manuela;

public class OnMacCatalyst : OnPlatform
{
    public OnMacCatalyst()
        : base(DevicePlatform.MacCatalyst)
    {

    }
}
