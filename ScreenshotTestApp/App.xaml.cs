using System.Configuration;
using System.Data;
using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using ScreenshotTestApp.Region_Selection;

namespace ScreenshotTestApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // base.OnStartup(e);
        // var builder = new ContainerBuilder();
        // builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
        // builder.RegisterType<RegionSelectViewModel>().As<IRegionSelectViewModel>().SingleInstance();
        //
        // var container = builder.Build();
        //
        // var selectionWindow = container.Resolve<RegionSelectionWindow>(); 
    }
}