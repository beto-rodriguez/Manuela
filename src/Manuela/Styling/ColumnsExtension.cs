// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Things;
using Microsoft.Maui.Layouts;

namespace Manuela;

[ContentProperty(nameof(Columns))]
public class ColumnsExtension : IMarkupExtension<FlexBasis>
{
    public int Columns { get; set; }

    public FlexBasis ProvideValue(IServiceProvider serviceProvider)
    {
        return new FlexBasis(Columns / ManuelaThings.MaxColumns, true);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
