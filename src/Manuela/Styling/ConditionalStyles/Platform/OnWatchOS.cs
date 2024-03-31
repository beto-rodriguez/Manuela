﻿// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling.ConditionalStyles.Platform;

namespace Manuela;

public class OnWatchOS : OnPlatform
{
    public OnWatchOS()
    : base(DevicePlatform.watchOS)
    {

    }
}
