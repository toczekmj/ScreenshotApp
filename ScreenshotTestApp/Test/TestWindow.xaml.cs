﻿using System.Windows;
using System.Windows.Media;
using ScreenshotTestApp.Tools;

namespace ScreenshotTestApp.Test;

public partial class TestWindow : Window
{
    private Color _defaultBackgroundColor = Colors.LightSeaGreen;
    private Color _secondaryBackgroundColor = Colors.LightPink;
    
    public TestWindow()
    {
        this.DataContext = new TestWindowViewModel();
        if(ViewModelHelper<ITestWindowViewModel>.TryGetViewModel(DataContext, out var viewModel))
        {
            viewModel!.BackgroundColor = _defaultBackgroundColor;
        }
        InitializeComponent();
    }
    
    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        var viewmodel = (TestWindowViewModel)this.DataContext;
    }

    private void Button_OnClick1(object sender, RoutedEventArgs e)
    {
        var viewmodel = (TestWindowViewModel)this.DataContext;
    }
}