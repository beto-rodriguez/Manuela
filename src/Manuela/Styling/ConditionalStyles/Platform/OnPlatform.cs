namespace Manuela.Styling.ConditionalStyles.Platform;

public class OnPlatform : ConditionalStyle
{
    public OnPlatform(DevicePlatform platform)
    {
        Condition = new(visualElement => DeviceInfo.Platform == platform)
        {
            Triggers = v => [new(v, [])]
        };
    }
}
