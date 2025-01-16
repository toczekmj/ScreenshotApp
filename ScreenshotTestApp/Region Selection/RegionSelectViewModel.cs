using System.Windows;
using System.Windows.Media;
using ScreenshotTestApp.Bases;
using ScreenshotTestApp.Commands;
using Color = System.Windows.Media.Color;

namespace ScreenshotTestApp.Region_Selection;

public class RegionSelectViewModel : BaseViewModel, IRegionSelectViewModel
{
#region properties
    private Color _backgroundColor = Colors.Brown;
    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (value != _backgroundColor)
                SetField(ref _backgroundColor, value);
        }
    }
    
    private WindowState _windowState = WindowState.Normal;
    public WindowState CurrentWindowState
    {
        get => _windowState;
        set
        {
            if(value != _windowState)
                SetField(ref _windowState, value);
        }
    }

#endregion

#region commands
    public RelayCommand ChangeBackgroundColorCommand => new(ChangeBackgroundColor);
    public RelayCommand ChangeWindowStateCommand => new(ChangeWindowState);

    private void ChangeBackgroundColor(object? parameter)
    {
        if (parameter is not Color color)
            return;
        
        BackgroundColor = color;
    }

    private void ChangeWindowState(object? parameter)
    {
        if (parameter is not WindowState state)
            return;
        CurrentWindowState = state;
    }
#endregion

}