namespace MSG.Android.LayoutGenerator.Extensions;


internal static class DebugHelper
{
    public static void WriteLog(string? value)
    {
#if DEBUG
        File.WriteAllText(Path.Combine(@"C:\Users\Egorozh\RiderProjects\MSG.Toolkit\Toolkit\MSG.Android.LayoutGenerator", "log.txt"), value);
#endif
    }
}