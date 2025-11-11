using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace Warehouse.Desktop.Components;

public partial class InputListComponent : UserControl
{
    public static readonly StyledProperty<object> LabelProperty =
        AvaloniaProperty.Register<InputListComponent, object>(nameof(Label));
    public object Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly StyledProperty<string> ValueProperty =
        AvaloniaProperty.Register<InputListComponent, string>(nameof(Value));
    public string Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<IEnumerable<string>> ItemsListProperty =
        AvaloniaProperty.Register<InputListComponent, IEnumerable<string>>(nameof(ItemsList));
    public IEnumerable<string> ItemsList
    {
        get => GetValue(ItemsListProperty);
        set => SetValue(ItemsListProperty, value);
    }
    public static readonly StyledProperty<AutoCompleteFilterPredicate<object?>?> FilterProperty =
        AvaloniaProperty.Register<InputListComponent, AutoCompleteFilterPredicate<object?>?>(nameof(Filter));

    public AutoCompleteFilterPredicate<object?>? Filter
    {
        get => GetValue(FilterProperty);
        set => SetValue(FilterProperty, value);
    }
    public InputListComponent()
    {
        InitializeComponent();
    }

}
