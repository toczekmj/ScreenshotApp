using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ScreenshotTestApp.Bases;
using ScreenshotTestApp.Commands;
using ScreenshotTestApp.Tools;
using ScreenshotTestApp.Tools.DllImporter;
using Color = System.Windows.Media.Color;

namespace ScreenshotTestApp.Region_Selection;

public class RegionSelectViewModel(string path) : BaseViewModel, IRegionSelectViewModel
{
    public RegionSelectViewModel() : this(string.Empty)
    {
    }

    #region properties

    private Color _backgroundColor = Colors.Gray;

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
            if (value != _windowState)
                SetField(ref _windowState, value);
        }
    }

    private Point _startingPoint = new(0, 0);
    private Point _relativeStartingPoint = new(0, 0);
    private Rectangle? _selectedRegion;

    

    #endregion

    #region commands definitions

    public RelayCommand ChangeBackgroundColorCommand => new(ChangeBackgroundColor);
    public RelayCommand ChangeWindowStateCommand => new(ChangeWindowState);
    public AsyncRelayCommand CapturePartialScreenshotAsyncCommand => new(CapturePartialScreenshotAsync);
    public RelayCommand StartSelectingAreaCommand => new(StartSelectingArea);
    public RelayCommand UpdateSelectedAreaCommand => new(UpdateSelectedArea);

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

    private async Task CapturePartialScreenshotAsync(object? parameter)
    {
        if (_selectedRegion is null)
        {
            return;
        }

        var mousePosition = CalculateTopLeftCorner();
        CurrentWindowState = WindowState.Minimized;
        _ = await ScreenShotHelper.CaptureRegionAsync(mousePosition.X, mousePosition.Y, _selectedRegion.Width,
            _selectedRegion.Height, path, ImageFormat.Png);
    }

    private void StartSelectingArea(object? parameter)
    {
        if (parameter is not WindowAndEventArgs { Window: { } window, EventArgs: MouseButtonEventArgs e })
        {
            return;
        }

        _relativeStartingPoint = e.GetPosition(window);
        _startingPoint = WinApiWrapper.GetMousePosition();
        _selectedRegion = new Rectangle
        {
            Stroke = Brushes.Red,
            StrokeThickness = 1,
            Fill = Brushes.Transparent
        };
        window.RegionSelectionCanvas.Children.Add(_selectedRegion);
    }

    private void UpdateSelectedArea(object? parameter)
    {
        if(parameter is not WindowAndEventArgs { Window: { } window, EventArgs: MouseEventArgs e })
        {
            return;
        }
        
        if(_selectedRegion is null || e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        var currentPoint = e.GetPosition(window);
        var x = Math.Min(currentPoint.X, _relativeStartingPoint.X);
        var y = Math.Min(currentPoint.Y, _relativeStartingPoint.Y);
        var width = Math.Abs(currentPoint.X - _relativeStartingPoint.X);
        var height = Math.Abs(currentPoint.Y - _relativeStartingPoint.Y);
        Canvas.SetLeft(_selectedRegion, x);
        Canvas.SetTop(_selectedRegion, y);
        _selectedRegion.Width = width;
        _selectedRegion.Height = height;
    }

    #endregion

    private Point CalculateTopLeftCorner()
    {
        var currentPosition = WinApiWrapper.GetMousePosition();
        return new Point(
            Math.Min(_startingPoint.X, currentPosition.X),
            Math.Min(_startingPoint.Y, currentPosition.Y)
        );
    }
}

public class WindowAndEventArgs
{
    public required RegionSelectWindow Window { get; init; }
    public required EventArgs EventArgs { get; init; }
}