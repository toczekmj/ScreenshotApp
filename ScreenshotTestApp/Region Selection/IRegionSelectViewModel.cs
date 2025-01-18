using System.Windows;
using System.Windows.Input;
using ScreenshotTestApp.Commands;
using Color = System.Windows.Media.Color;
namespace ScreenshotTestApp.Region_Selection;

public interface IRegionSelectViewModel
{
    public Color BackgroundColor { get; set; }
    public WindowState CurrentWindowState { get; set; }
    public RelayCommand ChangeBackgroundColorCommand { get; }
    public RelayCommand ChangeWindowStateCommand { get; }
    public AsyncRelayCommand CapturePartialScreenshotAsyncCommand { get; }
    public RelayCommand StartSelectingAreaCommand { get; }
    public RelayCommand UpdateSelectedAreaCommand { get; }
}