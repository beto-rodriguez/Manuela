namespace Manuela.Styling.ConditionalStyles;

public class Normal : ConditionalStyle
{
    public Normal()
    {
        Condition = new(visualElement => true)
        {
            Triggers = v => [new(v, [])]
        };
    }
}
