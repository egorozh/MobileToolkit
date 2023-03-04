using System.ComponentModel;
using Android.Text;
using SGM.Sample.Core;

namespace SGM.Sample.Android_;



[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    private MainViewModel _viewModel;
    
    private TextView _textView;
    private EditText _editText;


    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        _viewModel = new MainViewModel();

        _textView = this.FindViewById<TextView>(Resource.Id.TextView);
        var button = this.FindViewById<Button>(Resource.Id.Button);
        _editText = this.FindViewById<EditText>(Resource.Id.EditText);
        
        button.Click += ButtonOnClick;
        
        _editText.TextChanged += EditTextOnTextChanged;
        
        _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }

    
    private void EditTextOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        _viewModel.Text = _editText.Text;
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.Text))
        {
            _textView.Text = _viewModel.Text;
            _editText.Text = _viewModel.Text;
        }
    }


    private void ButtonOnClick(object? sender, EventArgs e)
    {
        _viewModel.ButtonCommand?.Execute(null);
    }
}