using ScreenshotTestApp.Commands;

namespace ScreenshotTestApp.MainWindow;

public interface IMainWindowViewModel
{
    public string DirectoryPath { get; set; }
    RelayCommand SelectDirectoryCommand { get; }
    RelayCommand PartialScreenShotCommand { get; }
    AsyncRelayCommand ScreenShotCommand { get; }
}