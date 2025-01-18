using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScreenshotTestApp.Tools;

/// <summary>
/// Helper class for capturing screenshots
/// </summary>
public static class ScreenShotHelper
{
    /// <summary>
    /// Captures a screenshot of the entire screen
    /// </summary>
    /// <param name="width"> Width of the screenshot </param>
    /// <param name="height"> Height of the screenshot </param>
    /// <param name="path"> Path to save the screenshot </param>
    /// <param name="format"> Image format to save the screenshot </param>
    /// <param name="ct"> Cancellation token </param>
    /// <returns> True if the screenshot was saved successfully, false otherwise </returns>
    /// <exception cref="OperationCanceledException"> If the operation was canceled </exception>
    /// <exception cref="Exception"> If the screenshot could not be saved </exception> 
    public static async Task<bool> CaptureScreenshotAsync(int width, int height, string path, ImageFormat format,
        CancellationToken ct = default)
    {
        try
        {
            // TODO: 0,0 relative to screen with mouse 
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

    /// <summary>
    /// Captures a screenshot of a region of the screen
    /// </summary>
    /// <param name="x"> X coordinate of the region </param>
    /// <param name="y"> Y coordinate of the region </param>
    /// <param name="width"> Width of the region </param>
    /// <param name="height"> Height of the region </param>
    /// <param name="path"> Path to save the screenshot </param>
    /// <param name="format"> Image format to save the screenshot </param>
    /// <param name="ct"> Cancellation token </param>
    /// <returns> True if the screenshot was saved successfully, false otherwise </returns>
    /// <exception cref="Exception"> If the screenshot could not be saved </exception>
    /// <exception cref="OperationCanceledException"> If the operation was canceled </exception>
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
    
    #region
    /// <summary>
    /// Captures a screenshot of a region of the screen
    /// </summary>
    /// <param name="x"> X coordinate of the region </param>
    /// <param name="y"> Y coordinate of the region </param>
    /// <param name="width"> Width of the region </param>
    /// <param name="height"> Height of the region </param>
    /// <param name="format"> Image format to save the screenshot </param>
    /// <param name="ct"> Cancellation token </param>
    /// <returns> The screenshot as a byte array </returns>
    /// <exception cref="Exception"> If the screenshot could not be saved </exception>
    /// <exception cref="OperationCanceledException"> If the operation was canceled </exception>
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

    /// <summary>
    /// Saves a byte array to a file
    /// </summary>
    /// <param name="ms"> Byte array to save </param>
    /// <param name="filePath"> Path to save the file </param>
    /// <param name="format"> Image format to save the file </param>
    /// <param name="ct"> Cancellation token </param>
    /// <returns> True if the file was saved successfully, false otherwise </returns>
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
    #endregion
}