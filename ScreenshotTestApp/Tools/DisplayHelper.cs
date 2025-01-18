using System.Windows;
using ScreenshotTestApp.Tools.DllImporter;

namespace ScreenshotTestApp.Tools;

public static class DisplayHelper
{
    public static void PositionWindowAtCursor(Window window)
    {
        var topLeftWpfCoordinate = GetWpfTopLeftCoordinate(
            WinApiWrapper.GetMousePosition(), 
            GetDpiScale(window)
            );
        window.Left = topLeftWpfCoordinate.X;
        window.Top = topLeftWpfCoordinate.Y;
    }

    #region
    private static Point GetWpfTopLeftCoordinate(Point cursorPosition, double dpiScale)
    {
        return new Point(cursorPosition.X / dpiScale, cursorPosition.Y / dpiScale);
    }
    
    private static double GetDpiScale(Window window)
    {
        var source = PresentationSource.FromVisual(window);
        return source?.CompositionTarget?.TransformToDevice.M11 ?? 1.0;
    }
    #endregion
}