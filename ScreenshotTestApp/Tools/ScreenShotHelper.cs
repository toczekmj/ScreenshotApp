using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScreenshotTestApp.Tools;

public static class ScreenShotHelper
{
    public static async Task<bool> CaptureScreenshotAsync(int width, int height, string path, ImageFormat format,
        CancellationToken ct = default)
    {
        try
        {
            var pixels = CaptureRegion(0, 0, width, height, format, ct);
            if (!await SaveFileAsync(pixels, path, format, ct))
            {
                throw new Exception("Failed to save screenshot");
            }
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine("Screenshot canceled");
            return false;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }

        return true;
    }

    public static async Task<bool> CaptureRegionAsync(double x, double y, double width, double height, string path, ImageFormat format,
        CancellationToken ct = default)
    {
        try
        {
            var pixels = CaptureRegion((int)x, (int)y, (int)width, (int)height, format, ct);
            if (!await SaveFileAsync(pixels, path, format, ct))
            {
                throw new Exception("Region save cancelled");
            }
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine("Region screenshot canceled");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        return true;
    }
    
    private static byte[] CaptureRegion(int x, int y, int width, int height, ImageFormat format,
        CancellationToken ct = default)
    {
        using MemoryStream memoryStream = new();
        try
        {
            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.CopyFromScreen(x, y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            bitmap.Save(memoryStream, format);
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"{width}x{height} could not be captured. Operation cancelled.");
            throw;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Failed to capture region: {e}");
            throw;
        }
        return memoryStream.ToArray();
    }

    private static async Task<bool> SaveFileAsync(byte[] ms, string filePath, ImageFormat format,
        CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) || !Directory.Exists(filePath))
            return false;

        await using var fileStream =
            File.Create(Path.Combine(filePath, Guid.NewGuid() + $".{format.ToString().ToLowerInvariant()}"));

        try
        {
            await fileStream.WriteAsync(ms, ct);
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"{filePath} was cancelled");
            return false;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }

        return true;
    }

    // private static byte[] CaptureScreen(int width, int height, ImageFormat format, CancellationToken ct = default)
    // {
    //     var upperLeftSource = new Point(0, 0);
    //     var upperLeftDestination = new Point(0, 0);
    //     using MemoryStream memoryStream = new();
    //
    //     try
    //     {
    //         using var bitmap = new Bitmap(width, height);
    //         using var graphics = Graphics.FromImage(bitmap);
    //
    //         graphics.CopyFromScreen(upperLeftSource, upperLeftDestination, bitmap.Size, CopyPixelOperation.SourceCopy);
    //         bitmap.Save(memoryStream, format);
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         Debug.WriteLine($"{width}x{height} could not be captured. Operation cancelled.");
    //         throw;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.WriteLine(e);
    //         throw;
    //     }
    //
    //     return memoryStream.ToArray();
    // }

    
}