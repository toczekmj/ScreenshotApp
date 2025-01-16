using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace ScreenshotTestApp.Tools;

public static partial class DisplayHelper
{
    public static void PositionWindowAtCursor(Window window)
    {
        var topLeftWpfCoordinate = GetWpfTopLeftCoordinate(
            GetMousePositionFromWinApi(), 
            GetDpiScale(window)
            );
        window.Left = topLeftWpfCoordinate.X;
        window.Top = topLeftWpfCoordinate.Y;
        // return new Point(topLeftWpfCoordinate.X, topLeftWpfCoordinate.Y);
    }

    private static Point GetWpfTopLeftCoordinate(Point cursorPosition, double dpiScale)
    {
        return new Point(cursorPosition.X / dpiScale, cursorPosition.Y / dpiScale);
    }
    
    private static double GetDpiScale(Window window)
    {
        var source = PresentationSource.FromVisual(window);
        return source?.CompositionTarget?.TransformToDevice.M11 ?? 1.0;
    }
    
    public static Point GetMousePositionFromWinApi()
    {
        var w32Mouse = new Win32Point();
        GetCursorPos(ref w32Mouse);
        
        return new Point(w32Mouse.X, w32Mouse.Y);
    }
    
    // TODO: error handling
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(ref Win32Point pt);

    [StructLayout(LayoutKind.Sequential)]
    private struct Win32Point
    {
        public Int32 X;
        public Int32 Y;
    }
}