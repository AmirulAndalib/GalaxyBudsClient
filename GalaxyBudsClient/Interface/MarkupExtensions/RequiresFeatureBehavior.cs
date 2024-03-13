using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using GalaxyBudsClient.Model.Specifications;
using GalaxyBudsClient.Platform;
using GalaxyBudsClient.Utils;

namespace GalaxyBudsClient.Interface.MarkupExtensions;

/// <summary>
/// Behavior that hides the control if the connected device does not support the specified feature.
/// </summary>
public class RequiresFeatureBehavior : Behavior<Control>
{
    public static readonly StyledProperty<IDeviceSpec.Feature> FeatureProperty =
        AvaloniaProperty.Register<RequiresFeatureBehavior, IDeviceSpec.Feature>(nameof(Feature));
    
    public static readonly StyledProperty<bool> NotProperty =
        AvaloniaProperty.Register<RequiresFeatureBehavior, bool>(nameof(Not));
    
    public IDeviceSpec.Feature Feature
    {
        get => GetValue(FeatureProperty);
        set => SetValue(FeatureProperty, value);
    }
    
    public bool Not
    {
        get => GetValue(NotProperty);
        set => SetValue(NotProperty, value);
    }
    
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        UpdateState();
        Settings.Instance.RegisteredDevice.PropertyChanged += OnDevicePropertyChanged;
    }
    
    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        Settings.Instance.RegisteredDevice.PropertyChanged -= OnDevicePropertyChanged;
    }

    private void OnDevicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateState();
    }
    
    protected bool State => BluetoothImpl.Instance.DeviceSpec.Supports(Feature) && !Not ||
                            !BluetoothImpl.Instance.DeviceSpec.Supports(Feature) && Not;
    protected virtual void UpdateState()
    {
        if (AssociatedObject is null)
            return;

        AssociatedObject.IsVisible = State;
    }
}