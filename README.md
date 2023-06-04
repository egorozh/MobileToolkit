[![Nuget](https://img.shields.io/nuget/v/MobileToolkit.Android?label=MobileToolkit.Android)](https://www.nuget.org/packages/MobileToolkit.Android)

# .NET Mobile Toolkit

## 1. Source generator for getting android controls from xml layout

Get started:

Install the library as a NuGet package:

```powershell
Install-Package MobileToolkit.Android
# Or 'dotnet add package MobileToolkit.Android'

```
And usage:
``` csharp

[Activity]
[AndroidLayout(LayoutResource = nameof(Resource.Layout.activity_main))]
public partial class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);
    }
}

```
The generated file looks like this:
``` csharp
using Android.Widget;
using Android.Views;


namespace SGM.Sample.Android
{
    partial class MainActivity
    {
        public TextView TextView => this.FindViewById<TextView>(Resource.Id.TextView);

        public Button Button => this.FindViewById<Button>(Resource.Id.Button);

        public EditText EditText => this.FindViewById<EditText>(Resource.Id.EditText);

        public View RecyclerView => this.FindViewById<View>(Resource.Id.RecyclerView);
    }
}
```



Plans:
1) Source generator for generate bindings code
2) Source generator for generate ui controls like android compose
3) Source generator for DI container
4) Source generator for generate outlets on .xib files
