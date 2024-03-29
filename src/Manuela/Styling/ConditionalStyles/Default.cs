namespace Manuela.Styling.ConditionalStyles;

public class Default : ConditionalStyle
{
    public Default()
    {
        Condition = new(visualElement => true)
        {
            Triggers = v => [new(v, [])]
        };
    }
}
