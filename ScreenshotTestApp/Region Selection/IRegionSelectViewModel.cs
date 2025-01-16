using System.Windows;
using ScreenshotTestApp.Commands;
using Color = System.Windows.Media.Color;
namespace ScreenshotTestApp.Region_Selection;

public interface IRegionSelectViewModel
{
    public Color BackgroundColor { get; set; }
    public WindowState CurrentWindowState { get; set; }
    public RelayCommand ChangeBackgroundColorCommand { get; }
    public RelayCommand ChangeWindowStateCommand { get; }
}