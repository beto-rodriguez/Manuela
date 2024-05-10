# Manuela gets things done.

![manuela](https://github.com/beto-rodriguez/Manuela/assets/10853349/37816487-7ac9-40b7-b255-23156ad54f71)

Manuela is a productivity framework for Maui; it is a quick way to get things done.

- Easily customize the look of the app on all platforms (including the title, status, and navigation bars).
- Flexible and XAML-only app layout: customizing the `Shell` class in Maui could be complicated and requires a lot of platform-specific code. Instead, Manuela uses a XAML only approach, you can build complex layouts with only XAML, which works on all platforms.
- Colors, sizes, margins, shadows, and more, quickly style the app as you need.
- build truly responsive apps. The current alternatives (like AdaptiveTrigger) require a lot of XAML to make them work properly; it seems too complex to share the same UI between desktop and mobile; now this is much easier.
- Forms, full control over how the Entry, Picker, Editor, CheckBox, RadioButton, and DatePicker look on all platforms.
- In my experience, validation on the client side is painful (on almost all frameworks). Manuela uses source generation to get all that boring and repetitive code for us... validate things and display the errors to the user just using DataAnnotations (similar to ASP).
- Transitions, currently Maui provides a limited way to build animations. Manuela provides a simple and intuitive syntax (similar to css) to define how property values transition over time.
- Dialogs, animated and customizable dialogs, quickly turn any XAML control into a dialog to prompt the user.

# Get started

From the command prompt, install the templates:

```
dotnet new install Manuela.Templates
```

That command will install 4 new templates:

```
Template Name          Short Name  Language  Tags
---------------------  ----------  --------  ------------
Manuela app            mn-app      [C#]      Maui/Manuela
Manuela empty app      mn-empty    [C#]      Maui/Manuela
Manuela hybrid         mn-hybrid   [C#]      Maui/Manuela
Manuela side menu app  mn-side     [C#]      Maui/Manuela
```

Restart visual studio, type Manuela in the search bar and you should see the new templates:

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/19bb1eca-64bf-4383-9ac6-fcc6ac403f23)

You must also import the namespace in xaml when required:

> xmlns:m="clr-namespace:Manuela;assembly=Manuela"

## Selecting a template

#### Manuela app template

The menu is at the left on medium or greater screens, it is at the bottom on phone, tablet and smaller screens.

![manuela-app](https://github.com/beto-rodriguez/Manuela/assets/10853349/6e010514-226f-4479-b631-90c381c37c58)

#### Manuela side menu app template

The user can toggle the side menu, when the screen is smaller than the "medium" size, the menu closes automatically.

![manuela-sidemenu](https://github.com/beto-rodriguez/Manuela/assets/10853349/f0ecbed1-8778-4861-8e12-7ec75fe2a473)

#### Manuela hybrid template

Just the Maui hybrid template, but wrapped in a ManuelaPage instance to remove borders and completely customize the the UI with HTML/Css.

![manuela-hybrid](https://github.com/beto-rodriguez/Manuela/assets/10853349/3a2cff05-6a5c-4397-a2f0-3b947f56b356)

#### Manuela Empty template

A template with a really basic Xaml template, ideal to build your own layout from scratch.

![manuela-empty](https://github.com/beto-rodriguez/Manuela/assets/10853349/56aa8c2a-10f4-4551-bd72-42522322272a)

#### Build a custom layout

All the previous templates are just XAML, Manuela is not using the `Shell` class, becuase it is hard to customize, you can find the source code of all those templates [here](https://github.com/beto-rodriguez/Manuela/tree/master/templates/content), specially take a look at the `AppLayout.xaml` and `AppLayout.xaml.cs` files, those files define the app structure.

# Manuela states

Manuela states is a way to simplify the required XAML to add responsivenes to the UI, there are 2 XAML extensions for this: the `States` and `Set` extensions.

The `States` extension, defines posible states of an UI element, the supported states are: `Disabled`, `Focused`, `Hovered`, `Pressed`, `Selected` (items inside a CollectionView), `Checked` (checkbox only) and `UnChecked` (checkbox only).

The `Set` extension defines the properties and values that will be assigned when a state is active.

```xml
<Button
    m:Has.States="{m:States
        Default={m:Set Background=Primary},
        Hovered={m:Set Background='Primary,Swatch200', TextColor='Gray,Swatch950'},
        Pressed={m:Set Background='Primary,Swatch900', TextColor='Gray,Swatch200'}}"/>
```

That previous code, changes the Background and TextColor of a Button depending if is hovered (pointer over), pressed or is on the default state, the required XAML for that without Manuela is... 😞

There are multiple things we can do with states, you can find more examples at [samples/Gallery/Views/States.xaml](https://github.com/beto-rodriguez/Manuela/blob/master/samples/Gallery/Views/States.xaml), or just clone this repo, run the gallery sample and see the States tab.

# Screen states

Actually there are more states that the ones listed above, the screen size states are activated depending on the screen size, the states are:

- `OnXs` < 576
- `OnSm` >= 576
- `OnMd` >= 768
- `OnLg` >= 1024
- `OnXl` >= 1280
- `OnXxl` >= 1536 _all values in device independent units_

```xml
<Button
    Text="This button changes background depending on the screen size"
    m:Has.States="{m:States
        OnXs=   {m:Set Background=Primary},
        OnSm=   {m:Set Background=Secondary},
        OnMd=   {m:Set Background=Tertiary},
        OnXl=   {m:Set Background=Gray},
        OnXxl=  {m:Set Background=Gradient}}">
</Button>
```

With this feature we can build complex user interfaces and at the same time understand the XAML we wrote 🎉🎉🎉 for example, take a look again at the gif in the **Manuela app template** example, that menu changes position depending on the screen size, this is the code that makes that happen:

```xml
<Border
    Background="White"
    m:Has.States="{m:States
        OnXs={m:Set
            AbsoluteLayoutFlags='WidthProportional,YProportional',
            AbsoluteLayoutBounds='0, 1, 1, 60',
            Padding='0',
            Margin='-1,0,-1,-1'},
        OnMd={m:Set
            AbsoluteLayoutFlags='HeightProportional',
            AbsoluteLayoutBounds='0, 0, 90, 1',
            Padding='0'}}" />
```

Clear isn't it? now the equivalent in Maui without manuela would be:

```xml
<Border Background="White">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup>
            <VisualState x:Name="Xs">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="0"/>
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Property="AbsoluteLayout.LayoutFlags" Value="WidthProportional,YProportional" />
                    <Setter Property="AbsoluteLayout.LayoutBounds" Value="0, 1, 1, 60" />
                    <Setter Property="Padding" Value="0" />
                    <Setter Property="Margin" Value="-1,0,-1,-1" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="Md">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="768"/>
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Property="AbsoluteLayout.LayoutFlags" Value="HeightProportional" />
                    <Setter Property="AbsoluteLayout.LayoutBounds" Value="0, 0, 90, 1" />
                    <Setter Property="Padding" Value="0" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</Border>
```

Maybe I am too picky, but for me, it takes much more time to read and understand the AdaptiveTriggers syntax, for me, it is always important to understand quickly what you wrote, specially when you come back to the code you wrote 5 years ago...

There are also a couple of useful properties to configure screen states:

- Force{bp}OnIdiom: forces the state to be true on a certain idiom (Desktop,Phone,Tablet,TV, Watch), a common practical case, is when you want all tablets and phones to have the same style, ignoring the size of the screen.

```xml
here the padding 0 is used on Xs screens
on phones and tablets, the screen size will be ignored, padding value is also 0
finally on Md screens and greater, the padding is 25

<Setter
    Property="m:Has.States"
    Value="{m:States
        OnXs={m:Set Padding='0'}, ForceXsOnIdiom='Phone,Tablet',
        OnMd={m:Set Padding='25'}}"/>
```

- {bp}MaxBreakpoint: configures the max screen size where the State is applied to, default is Xxl.

```xml
here the padding is only applied on XS screens.

<Setter
    Property="m:Has.States"
    Value="{m:States
        OnXs={m:Set Padding='0'},
        XsMaxBreakpoint=Xs/>
```

# Transitions

Manuela provides a quick way to create animations, all the animations you saw in the templates sections was build with the Manuela API, the idea is inspired on Css and LiveCharts :).
You must indicate the property name, then when that property changes Manuela will animate things for you, is that simple.
We can mix it with the states feature above, for example, lets rotate a button when the user presses it:

```xml
<Button
    Text="press me and hold"
    m:Has.Transitions="{m:Transitions 'Scale,Rotation'}"
    m:Has.States="{m:States
        Default={m:Set Scale=1, Rotation=0},
        Pressed={m:Set Scale=1.5, Rotation=45}}">
</Button>
```

![manuela-transitons](https://github.com/beto-rodriguez/Manuela/assets/10853349/2ac49473-bbf4-42e3-aa7c-588430379806)

That uses the default settings, but we can also specify the Easing curve and duration of the animation:

```xml
<Button
    Text="press me and hold"
    m:Has.Transitions="{m:Transitions
        Scale={m:TransitionDefinition Easing={x:Static Easing.BounceOut},Duration=1200},
        Rotation={m:TransitionDefinition Easing={x:Static Easing.SpringOut},Duration=1200}}"
    m:Has.States="{m:States
        Default={m:Set Scale=1, Rotation=0},
        Pressed={m:Set Scale=1.5, Rotation=45}}">
</Button>
```

![manuela-transitons-bounce](https://github.com/beto-rodriguez/Manuela/assets/10853349/f0c380ed-83bb-40ea-a5a9-0e6ba7d7d718)

# Forms

Just some input controls that look the same on all platforms, they are easy to customize, for more info see [/samples/Gallery/Views/Forms.xaml](https://github.com/beto-rodriguez/Manuela/blob/master/samples/Gallery/Views/Forms.xaml).

These control consume more memory than regular Maui Controls, because each control uses multiple Maui controls, in general performance is nice if your forms are small (less than 30~50 inputs) if you need more than that, consider using pagination.

![forms](https://github.com/beto-rodriguez/Manuela/assets/10853349/4b77d160-2ee7-428d-8d91-9e6ddaa4af87)

```xml
... also import the ns
xmlns:forms="clr-namespace:Manuela.Forms;assembly=Manuela"

<forms:TextInput Placeholder="Enter some text here"/>

<forms:TextAreaInput Placeholder="Intert multiple lines here" />

<forms:PickerInput Placeholder="Select an option">
    <forms:PickerInput.ItemsSource>
        <x:Array Type="{x:Type x:String}">
            <x:String>This is an option</x:String>
            <x:String>This is another one</x:String>
            <x:String>Pick me</x:String>
            <x:String>Please pick me!</x:String>
        </x:Array>
    </forms:PickerInput.ItemsSource>
</forms:PickerInput>

<forms:DatePickerInput Placeholder="Select a date"/>

<forms:CheckBoxInput Placeholder="Check me"/>

<StackLayout>
    <Label Text="What's your favorite animal?" />
    <forms:RadioButtonInput Content="Cat" />
    <forms:RadioButtonInput Content="Dog" />
    <forms:RadioButtonInput Content="Elephant" />
    <forms:RadioButtonInput Content="Monkey" IsChecked="true" />
</StackLayout>
```

#### input groups

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/7a9ac9f5-95d9-4197-b2df-0f7dd45b5bc5)

```xml
<forms:InputGroup>
    <forms:InputGroup.LeftContent>
        <mi:MauiIcon Icon="{mi:SegoeFluent Icon=Phone}" IconSize="25" Margin="{m:Spacing Left=Lg}" />
    </forms:InputGroup.LeftContent>

    <forms:InputGroup.MiddleContent>
        <forms:TextInput  Placeholder="Enter something here"/>
    </forms:InputGroup.MiddleContent>

    <forms:InputGroup.RightContent>
        <mi:MauiIcon Icon="{mi:SegoeFluent Icon=Mail}" IconSize="25" Margin="{m:Spacing Right=Lg}"/>
    </forms:InputGroup.RightContent>
</forms:InputGroup>
```

more samples at: [/samples/Gallery/Views/Forms.xaml](https://github.com/beto-rodriguez/Manuela/blob/master/samples/Gallery/Views/Forms.xaml)

# Validation

Manuela can validate inputs based on the DataAnnotation attributes in a model.

```c#
public class Customer
{
    [Display(Name = "Please inster a name")]
    [Required]
    [MinLength(10)]
    public string? Name { get; set; } = "juan";

    // ...
    // full file at Views/ValidationObjects/Customer.cs
}
```

Then, we need to create a Form for our customer class, Manuela source generator will setup this form so we can just bind it to the UI inputs.

```c#
public partial class CustomerForm : Form&lt;Customer>
{
    // when we inherit from Form&lt;T>, manuela will generate the form for us.
    // now we are ready to use this form as the BindingContext of our UI.

    // we can also add custom methods/commands here, for example lets add a command to save the customer.
    public Command SaveCommand => new(() =>
    {
        if (!IsValid()) return;

        // the model is valid at this point.
        // save the record maybe?
        var customer = Model;
    });

    // full file at Views/ValidationObjects/CustomerForm.cs
}
```

Finally use the form in the UI, the validation error will be shown after 800ms without changes in the input.

```xml
<f:TextInput For="{Binding Name}"/>
<f:TextInput For="{Binding Email}"/>
<f:DatePickerInput For="{Binding BirthDate}"/>
...
```

![validate](https://github.com/beto-rodriguez/Manuela/assets/10853349/6115a341-2fce-4323-9649-1910b8827f8f)

# Dialogs

Just awaitable, flexible, animated dialogs to quickly promt the user for any kinf or response:

```xml
<Button
    Text="Default dialog answers"
    Clicked="ShowDefaultDialog"/>
```

```c#
private async void ShowDefaultDialog(object sender, EventArgs e)
{
    var answer = await Modal.Show(
        "Select an option",
        "Modals can be awaited to know the user result, please select an option:",
        ModalOptions.YesNoCancel);

    var message = $"you picked {answer}";
}
```

![dialogs](https://github.com/beto-rodriguez/Manuela/assets/10853349/ed8d8536-1635-4071-8130-03e71757a20b)

Dialogs are super easy to customize, you can use any `ContentView` as a dialog, in the next exammple we use [this xaml](https://github.com/beto-rodriguez/Manuela/blob/master/samples/Gallery/Views/CustomDialogs/MyModalPicker.xaml), the important part
is that when the user select an item in the Collection view we set the dialog response, this will complete the awaited task.

```c#
private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    // when a view is displayed in a modal, a TaskCompletionSource is attached to the view.
    // you can use the SetDialogResponse extension method to set the result of the assigned TCS.

    this.SetDialogResponse(e.CurrentSelection.FirstOrDefault());
}
```

![dialogs-custom](https://github.com/beto-rodriguez/Manuela/assets/10853349/d3116744-e629-4247-85be-e593d43db341)

And yes, dialogs are nestable:

![dialogs-nested](https://github.com/beto-rodriguez/Manuela/assets/10853349/ea17f390-a4c7-403e-9b45-d27c492ea6ac)


# Colors, Brushes and Gradients

There are 4 base colors, `Primary`, `Secondary`, `Tertiary` and `Gray`.
Each color has 11 swatches: `50`, `100`, `200`, `300`, `400`, `500`, `600`, `700`, `800`, `900`, `950`, where `50` is the closest to the background color and `950` is the closest to the foreground color, this means that `950` is the color with the most contrast no matter the if the theme is dark or light, also `50` is the color with the less contrast no matter the the theme.

```xml
<BoxView Background="{m:Brush Primary}" />
<BoxView Background="{m:Brush 'Primary,Swatch50'}"/>
<BoxView BackgroundColor="{m:Color Secondary}" />
<BoxView BackgroundColor="{m:Color 'Secondary,Swatch950'}"/>
```

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/476fb26e-6e95-411d-840f-67df53eaff84)

### Gradients

```xml
<BoxView Background="{m:Brush 'Secondary,Gradient'}" />
<BoxView Background="{m:Brush 'Secondary,Gradient,GradientSm'}" />
<BoxView Background="{m:Brush 'Secondary,Gradient,GradientLg'}" />
<BoxView Background="{m:Brush 'Primary,Gradient,GradientLg,GradientY'}" />
<BoxView Background="{m:Brush 'Primary,Gradient,GradientLg,GradientX,GradientInvert'}" />
```

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/b292b3b1-6f36-403f-af75-432535eda3db)

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/3dee4e6f-f524-46cc-9ce9-982dd1957223)

### Opacity

```xml
<BoxView Background="{m:Brush 'Primary,Opacity10'}"/>
<BoxView Background="{m:Brush 'Primary,Swatch500,Opacity90'}"/>
```

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/7a91b6bf-8436-4c27-8ed0-f49473fa69dc)

### Color types

Somethimes Brushes are not supported, for example the Label TextColor property is of type Color, in those cases, you can use the Color extenstion, it has the same support as Brushes (but gradients).

```xml
<Label TextColor="{m:Color Primary}" />
<Label TextColor="{m:Color Secondary}" />
<Label TextColor="{m:Color Tertiary}" />
<Label TextColor="{m:Color Gray}" />
```

![image](https://github.com/beto-rodriguez/Manuela/assets/10853349/8e7b0e72-6b9f-4769-b7fc-492556cbb0c8)

# Sizing

The Size extension provides 9 different sizes, `None`, `Xs`, `Sm`, `Md`, `Lg`, `Xl`, `Xxl`, `Huge`, `Giant` and `Titatanic`, You can use it to quickly set `margins`, `paddings`, `borders`, `border radius`, `shadows`, `text sizes`, `line heights`, and the `dimension` of visual elements, depending on the property type, each value is resolved, e.g. it means `Xs` value is different when the property is `FontSize` or when it is `Margin`.

```xml
<BoxView Margin="{m:Size Xxl}" />
<Button Padding="{m:Size Xxl}" Text="Click me"/>
<Button BorderWidth="{m:Size Xxl}" Text="Click me"/>
<Button CornerRadius="{m:Size Xxl}" Text="Click me"/>
<BoxView Shadow="{m:Size Xl}"/>
<Label Text="Hello" FontSize="{m:Size Xs}"/>
<Label Text="Hello" LineHeight="{m:Size None}"/>
<BoxView WidthRequest="{m:Size Lg}" HeightRequest="{m:Size Lg}"/>
<HorizontalStackLayout Spacing="{m:Size Md}"> ... </HorizontalStackLayout>
```

# Spacing

The spacing extension is to build not even margings or paddings:

```xml
<BoxView Margin="{m:Spacing Horizontal=Huge}" />
<BoxView Margin="{m:Spacing Vertical=Huge}" />

<BoxView Margin="{m:Spacing Right=Huge}" />
<BoxView Margin="{m:Spacing Top=Huge}" />
<BoxView Margin="{m:Spacing Bottom=Huge}" />
<BoxView Margin="{m:Spacing Left=Huge}" />
```

You can mix properties, the spacing is applied from the less specific to the most specific property.
The Size (uniform) is applied first, then the Vertical and Horizontal sizes, finally the Left, Top, Right and Bottom.
This means that you can for example set all the properties to large but override the top to None.

```xml
<BoxView Margin="{m:Spacing Size=Xl, Top=None}" />
```

