using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ScreenshotTestApp.Tools;

namespace ScreenshotTestApp.Region_Selection;
// TODO: clean up this mess 
public partial class RegionSelectWindow : Window
{
    private Point _startingPoint = new(0, 0);
    private Point _relativeStartingPoint = new(0, 0);
    private Rectangle? _selectedRegion;
    private string _path;

    public RegionSelectWindow(string path)
    {
        _path = path;
        this.DataContext = new RegionSelectViewModel();
        InitializeComponent();
    }

    public void MoveToMousePosition()
    {
        DisplayHelper.PositionWindowAtCursor(this);
    }

    public void MaximizeWindow()
    {
        if(ViewModelHelper<RegionSelectViewModel>.TryGetViewModel(DataContext, out var viewModel))
        {
            viewModel!.CurrentWindowState = WindowState.Maximized;
        }
    }

    private async void RegionSelectWindow_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        try
        { 
            if (_selectedRegion is null)
                return;
            
            var mousePosition = CalculateTopLeftCorner(DisplayHelper.GetMousePositionFromWinApi());
            if(ViewModelHelper<IRegionSelectViewModel>.TryGetViewModel(DataContext, out var viewModel))
            {
                viewModel!.CurrentWindowState = WindowState.Minimized;
            }
            _ = await ScreenShotHelper.CaptureRegionAsync(mousePosition.X, mousePosition.Y, _selectedRegion.Width, _selectedRegion.Height, _path, ImageFormat.Png);
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
        finally
        {
            Close();
        }
    }

    // TODO: move this to viewmodel
    private Point CalculateTopLeftCorner(Point currentPoint) => 
        new( Math.Min(_startingPoint.X, currentPoint.X), Math.Min(_startingPoint.Y, currentPoint.Y));
    
    // TODO: probably this also 
    private void RegionSelectWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        _relativeStartingPoint = e.GetPosition(this);
        _startingPoint = DisplayHelper.GetMousePositionFromWinApi();
        _selectedRegion = new Rectangle
        {
            Stroke = Brushes.Red,
            StrokeThickness = 1,
            Fill = Brushes.Transparent
        };
        RegionSelectionCanvas.Children.Add(_selectedRegion);
    }
    // TODO: And this
    private void RegionSelectWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_selectedRegion is null || e.LeftButton != MouseButtonState.Pressed)
            return;
        
        var currentPoint = e.GetPosition(this);
        var x = Math.Min(currentPoint.X, _relativeStartingPoint.X);
        var y = Math.Min(currentPoint.Y, _relativeStartingPoint.Y);
        var width = Math.Abs(currentPoint.X - _relativeStartingPoint.X);
        var height = Math.Abs(currentPoint.Y - _relativeStartingPoint.Y);

        Canvas.SetLeft(_selectedRegion, x);
        Canvas.SetTop(_selectedRegion, y);
        _selectedRegion.Width = width;
        _selectedRegion.Height = height;
    }
}