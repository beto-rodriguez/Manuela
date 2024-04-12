
namespace Manuela.States.Device;

public class OnIdiom : ConditionalStyle
{
    public OnIdiom(DeviceIdiom idiom)
    {
        Condition = new(visualElement => DeviceInfo.Idiom == idiom);
    }

    protected override void OnInitialized(VisualElement visualElement)
    {
        Condition.Triggers = [new(visualElement, [])];
    }
}
