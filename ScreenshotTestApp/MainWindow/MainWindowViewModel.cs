using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ScreenshotTestApp.Bases;
using ScreenshotTestApp.Commands;
using ScreenshotTestApp.Region_Selection;
using ScreenshotTestApp.Tools;

namespace ScreenshotTestApp.MainWindow;

public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
{
    private string _directoryPath = string.Empty;
    public string DirectoryPath
    {
        get => _directoryPath;
        set => SetField(ref _directoryPath, value);
    }

    public RelayCommand SelectDirectoryCommand => new(SelectFolder);
    public AsyncRelayCommand ScreenShotCommand => new(PrintScreenAsync);
    public RelayCommand PartialScreenShotCommand => new(PartialPrintScreen);
    
    #region commands definitions
    private async Task PrintScreenAsync(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(DirectoryPath))
            return;
        
        if (string.IsNullOrWhiteSpace(_directoryPath) || !Directory.Exists(Path.GetDirectoryName(_directoryPath)))
        {
            MessageBox.Show("Invalid path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        try
        {
            _ = await ScreenShotHelper.CaptureScreenshotAsync(1920, 1080, _directoryPath, ImageFormat.Png);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void PartialPrintScreen(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(_directoryPath))
        {
            return;
        }

        var overlay = new RegionSelectWindow(_directoryPath);
        overlay.Show();
        overlay.MoveToMousePosition();
        overlay.MaximizeWindow();
    }
    private void SelectFolder(object? parameter)
    {
        var openFolderDialog = new OpenFolderDialog
        {
            Title = "Select Folder",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        };
        if (openFolderDialog.ShowDialog() == true)
        {
            DirectoryPath = openFolderDialog.FolderName + "\\";
            return;
        }
        DirectoryPath = string.Empty;
    }
    #endregion
}