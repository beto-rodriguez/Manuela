// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.States.Device;

namespace Manuela;

public class OnPhone : OnIdiom
{
    public OnPhone()
        : base(DeviceIdiom.Phone)
    {

    }
}
