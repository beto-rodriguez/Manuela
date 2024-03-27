namespace Manuela.Styling.ConditionalStyles.Device;

public class OnIdiom : ConditionalStyle
{
    public OnIdiom(DeviceIdiom idiom)
    {
        Condition = new(visualElement => DeviceInfo.Idiom == idiom)
        {
            Triggers = v => [new(v, [])]
        };
    }
}
