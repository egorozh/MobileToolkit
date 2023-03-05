using System;

namespace MSG.Android.LayoutGenerator
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
    internal class AndroidLayoutGenerateAttribute : Attribute
    {
        public AndroidLayoutGenerateAttribute(int layoutResource, string? source = null)
        {
        }
    }
}