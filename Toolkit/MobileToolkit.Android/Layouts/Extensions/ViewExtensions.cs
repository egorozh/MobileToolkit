using Android.Views;


namespace MobileToolkit.Android.Layouts.Extensions;


public static class ViewExtensions
{
    public static T As<T>(this View view) where T : View
    {
        return (T) view;
    }
}