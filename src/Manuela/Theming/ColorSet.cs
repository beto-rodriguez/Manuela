namespace Manuela.Theming;

public class ColorSet : Dictionary<UIBrush, Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSet"/> class.
    /// </summary>
    public ColorSet()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSet"/> class.
    /// </summary>
    /// <param name="primary">The primary colors set.</param>
    /// <param name="secondary">The secondary colors set.</param>
    /// <param name="tertiary">The tertirary colors set.</param>
    /// <param name="gray">The gray colors set.</param>
    /// <param name="defaultIndex">The index of the element that contains the default color in the swatches collection.</param>
    public ColorSet(string[] primary, string[] secondary, string[] tertiary, string[] gray, int defaultIndex = 5)
    {
        _ = this
            .AddPallete(UIThemeColor.Primary, primary, defaultIndex)
            .AddPallete(UIThemeColor.Secondary, secondary, defaultIndex)
            .AddPallete(UIThemeColor.Tertiary, tertiary, defaultIndex)
            .AddPallete(UIThemeColor.Gray, gray, defaultIndex);
    }
}
