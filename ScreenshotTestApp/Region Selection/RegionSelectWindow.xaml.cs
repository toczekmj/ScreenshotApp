using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ScreenshotTestApp.Tools;

namespace ScreenshotTestApp.Region_Selection;

public partial class RegionSelectWindow : Window
{
    public RegionSelectWindow(string path)
    {
        InitializeComponent();
        DataContext = new RegionSelectViewModel(path);
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

    private void RegionSelectWindow_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (ViewModelHelper<RegionSelectViewModel>.TryGetViewModel(DataContext, out var viewModel))
            {
                
                viewModel!.CapturePartialScreenshotAsyncCommand.Execute(null);
            }
            else
            {
                throw new Exception("ViewModel not found");
            }
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
    
    private void RegionSelectWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            var parameter = new WindowAndEventArgs
            {
                Window = this,
                EventArgs = e
            };
            if (ViewModelHelper<RegionSelectViewModel>.TryGetViewModel(DataContext, out var viewModel))
            {
                viewModel!.StartSelectingAreaCommand.Execute(parameter);
            }
            else
            {
                throw new Exception("ViewModel not found");
            }
        }
        catch
        {
            Debug.WriteLine("Error occurred while trying to start selecting area");
        }
    }
    
    private void RegionSelectWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        try
        {
            var parameter = new WindowAndEventArgs
            {
                Window = this,
                EventArgs = e
            };
            if (ViewModelHelper<RegionSelectViewModel>.TryGetViewModel(DataContext, out var viewModel))
            {
                viewModel!.UpdateSelectedAreaCommand.Execute(parameter);
            }
            else
            {
                throw new Exception("ViewModel not found");
            }
        }
        catch
        {
            Debug.WriteLine("Error occurred while trying to update selected area");
        }
    }
}