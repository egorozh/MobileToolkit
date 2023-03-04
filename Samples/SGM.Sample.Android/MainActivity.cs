using System.ComponentModel;
using Android.Text;
using SGM.Android.LayoutGenerator;
using SGM.Sample.Core;


namespace SGM.Sample.Android;


[Activity(Label = "@string/app_name", MainLauncher = true)]
[AndroidLayoutGenerate(Resource.Layout.activity_main)]
public partial class MainActivity : Activity
{
    private MainViewModel _viewModel;
    

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        _viewModel = new MainViewModel();

        InitializeControls();
        
        Button.Click += ButtonOnClick;
        
        EditText.TextChanged += EditTextOnTextChanged;
        
        _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }
    
    
    private void EditTextOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        _viewModel.Text = EditText.Text;
    }

    
    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.Text))
        {
            TextView.Text = _viewModel.Text;
            EditText.Text = _viewModel.Text;
        }
    }


    private void ButtonOnClick(object? sender, EventArgs e)
    {
        _viewModel.ClickCommand?.Execute(null);
    }
}