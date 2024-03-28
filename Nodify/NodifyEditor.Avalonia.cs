namespace Nodify;

public partial class NodifyEditor
{
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == DisplayConnectionsOnTopProperty)
            PseudoClasses.Set(":connections-on-top", DisplayConnectionsOnTop);
    }

    /// <summary>
    /// On WPF single mouse wheel delta is 120, on Avalonia it is 0.6 (at least on macOS)
    /// </summary>
    private double MouseWheelAvaloniaToWpfScale => 120 / 0.6;
    
    private double PointerTouchGestureMagnifyScale => 4;

    private void OnPointerTouchPadGestureMagnify(object? sender, PointerDeltaEventArgs e)
    {
        State.HandleMouseWheel(new MouseWheelEventArgs());

        if (!e.Handled && EditorGestures.Zoom == e.KeyModifiers)
        {
            double zoom = Math.Pow(2.0, e.Delta.Y / 3.0 / MouseWheelDeltaForOneLine * MouseWheelAvaloniaToWpfScale * PointerTouchGestureMagnifyScale);
            ZoomAtPosition(zoom, e.GetPosition(ItemsHost));

            // Handle it for nested editors
            if (e.Source is NodifyEditor)
            {
                e.Handled = true;
            }
        }
    }
}