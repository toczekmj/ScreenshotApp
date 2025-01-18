namespace ScreenshotTestApp.Tools;

/// <summary>
/// Provides helper methods for working with view models.
/// </summary>
/// <typeparam name="T">The type of the view model.</typeparam>
public static class ViewModelHelper<T> where T : class
{
    
    /// <summary>
    /// Gets the view model from the specified data context.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns> The view model of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">If the data context is not of the specified type.</exception>
    public static T GetViewModel(object dataContext)
    {
        if (dataContext is not T viewModel)
        {
            throw new ArgumentException($"DataContext must be of type {typeof(T).Name}");
        }
        return viewModel;
    }

    /// <summary>
    /// Tries to get the view model from the specified data context.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="viewModel"> The view model of type <typeparamref name="T"/>.</param>
    /// <returns><see langword="true"/> if the view model was successfully retrieved; otherwise, <see langword="false"/>.</returns>
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