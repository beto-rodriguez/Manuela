// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States.Platform;

namespace Manuela;

public class OnMacOS : OnPlatform
{
    public OnMacOS()
        : base(DevicePlatform.macOS)
    {

    }
}
