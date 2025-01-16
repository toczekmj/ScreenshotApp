using ScreenshotTestApp.Region_Selection;

namespace ScreenshotTestApp.Tools;

public static class ViewModelHelper<T> where T : class
{
    public static T GetViewModel(object dataContext)
    {
        if (dataContext is not T viewModel)
        {
            throw new ArgumentException($"DataContext must be of type {typeof(T).Name}");
        }
        return viewModel;
    }
}

public static class RegionSelectViewModelExtensions
{
    public static RegionSelectViewModel GetViewModelInstance(this object dataContext)
    {
        if (dataContext is not RegionSelectViewModel viewModel)
        {
            throw new ArgumentException($"DataContext must be of type {nameof(RegionSelectViewModel)}");
        }
        return viewModel;
    }
}