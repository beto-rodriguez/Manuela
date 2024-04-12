
namespace Manuela.States.Platform;

public class OnPlatform : ConditionalStyle
{
    public OnPlatform(DevicePlatform platform)
    {
        Condition = new(visualElement => DeviceInfo.Platform == platform);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [])];
    }
}
