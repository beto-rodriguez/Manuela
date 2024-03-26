namespace Manuela.Styling.ConditionalStyles;

public class Selected : ConditionalStyle
{
    public Selected()
    {
        Condition = new(visualElement => false)
        {
            Triggers = v =>
            {
                // could more states use Maui VisualStates?
                // yes... but I really prefer Manuela implementation, it just subscribes to the property changed event.
                // but in the case of the "Selected" state, there is no Bindable/Attached property to subscribe to.
                // Maybe Maui should add an attached property for this case???

                var selectedState = new VisualState { Name = "Selected" };
                foreach (var item in Setters?.AsMauiSetters(v) ?? [])
                    selectedState.Setters.Add(item);

                v.SetValue(
                    VisualStateManager.VisualStateGroupsProperty,
                    new VisualStateGroupList
                    {
                        new VisualStateGroup
                        {
                            Name = "CommonStates",
                            States =
                            {
                                new VisualState { Name = "Normal" },
                                selectedState
                            }
                        }
                    });

                return [new(v, [])];
            }
        };
    }
}
