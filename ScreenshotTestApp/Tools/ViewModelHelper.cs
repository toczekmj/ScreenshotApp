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

    public static bool TryGetViewModel(object dataContext, out T? viewModel)
    {
        if (dataContext is not T model)
        {
            viewModel = null;
            return false;
        }
        viewModel = model;
        return true;
    }
}