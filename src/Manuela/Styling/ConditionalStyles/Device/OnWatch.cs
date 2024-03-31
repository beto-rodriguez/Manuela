// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles.Device;

namespace Manuela;

public class OnWatch : OnIdiom
{
    public OnWatch()
        : base(DeviceIdiom.Watch)
    {

    }
}
