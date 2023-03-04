using Android.Views;


namespace MSG.Android.LayoutGenerator;


internal static class ViewExtensions
{
    public static T As<T>(this View view) where T : View
    {
        return (T)view;
    }
}