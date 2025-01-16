using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using ScreenshotTestApp.Bases;
using ScreenshotTestApp.Commands;
using Color = System.Windows.Media.Color;
namespace ScreenshotTestApp.Test;

public interface ITestWindowViewModel
{
    public Color BackgroundColor { get; set; }
    public RelayCommand ChangeColorCommand { get; }
}

public class TestWindowViewModel : BaseViewModel, ITestWindowViewModel
{
    private Color _backgroundColor = Colors.Fuchsia;

    public RelayCommand ChangeColorCommand => new(OnColorChange);

    public Color BackgroundColor
    {
        get => _backgroundColor;
        set => SetField(ref _backgroundColor, value);
    }
    
    public TestWindowViewModel()
    {
        PropertyChanged += PropertyChanged_OnChange;
    }

    private void PropertyChanged_OnChange(object? sender, PropertyChangedEventArgs e)
    {
        Debug.WriteLine($"Property {e.PropertyName} has changed to: {_backgroundColor}");
    }

    private void OnColorChange(object? c)
    {
        if (c is string colorName)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                BackgroundColor = color;
            }
            catch (FormatException)
            {
                Debug.WriteLine($"Invalid color name: {colorName}");
            }
        }
    }
}




