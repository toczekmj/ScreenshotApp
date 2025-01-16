using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using ScreenshotTestApp.Region_Selection;
using ScreenshotTestApp.Test;
using ScreenshotTestApp.Tools;
using Path = System.IO.Path;

namespace ScreenshotTestApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void WholeScreenPrintScreenButton_OnClick(object sender, RoutedEventArgs e)
    {
        var savePath = SavePathTextBox.Text;
        if (string.IsNullOrWhiteSpace(savePath) || !Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            MessageBox.Show("Invalid path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        try
        {
            var result = await ScreenShotHelper.CaptureScreenshotAsync(1920, 1080, savePath, ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PartialScreenPrintScreenButton_OnClick(object sender, RoutedEventArgs e)
    {
        new RegionSelectWindow(SavePathTextBox.Text).ShowDialog();
    }

    private void Button_OnClick(object sendter, RoutedEventArgs e)
    {
        OpenFolderDialog folderDialog = new OpenFolderDialog()
        {
            Title = "Select Folder",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        };
        if (folderDialog.ShowDialog() == true)
        {
            SavePathTextBox.Text = folderDialog.FolderName + "\\";
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var savePath = SavePathTextBox.Text;
        if (string.IsNullOrWhiteSpace(savePath) || !Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            MessageBox.Show("Invalid path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var testWindow = new RegionSelectWindow(savePath);
        testWindow.Show();
        testWindow.MoveToMousePosition();
        testWindow.MaximizeWindow();
    }
}