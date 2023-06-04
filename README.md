[![Nuget](https://img.shields.io/nuget/v/MSG.AndroidToolkit?label=MSG.AndroidToolkit)](https://www.nuget.org/packages/MSG.AndroidToolkit)

# Dotnet Mobile Toolkit

## 1. Source generator for getting android controls from xml layout

Get started:

Install the library as a NuGet package:

```powershell
Install-Package MSG.AndroidToolkit
# Or 'dotnet add package MSG.AndroidToolkit'

```
And usage:
``` csharp

[Activity]
[AndroidLayoutGenerate(LayoutResource = nameof(Resource.Layout.activity_main))]
public partial class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        InitializeControls();
    }
}

```
The generated file looks like this:
``` csharp
using Android.Widget;
using Android.Views;


namespace SGM.Sample.Android;


partial class MainActivity
{
    protected TextView TextView { get; private set; }

    protected Button Button { get; private set; }

    protected EditText EditText { get; private set; }


    protected void InitializeControls()
    {
        TextView = this.FindViewById<TextView>(Resource.Id.TextView);

        Button = this.FindViewById<Button>(Resource.Id.Button);

        EditText = this.FindViewById<EditText>(Resource.Id.EditText);

    }

}
```



Plans:
1) Source generator for generate bindings code
2) Source generator for generate ui controls like android compose
3) Source generator for DI container
4) Source generator for generate outlets on .xib files
