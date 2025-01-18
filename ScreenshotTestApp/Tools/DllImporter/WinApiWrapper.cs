using System.Runtime.InteropServices;
using System.Windows;

namespace ScreenshotTestApp.Tools.DllImporter;

public static class WinApiWrapper
{
    public static Point GetMousePosition()
    {
        var w32Mouse = new Win32Point();
        GetCursorPos(ref w32Mouse);
        return new Point(w32Mouse.X, w32Mouse.Y);
    }
    
    #region ddl imports
    
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(ref Win32Point pt);
    
    #endregion
}