using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SGM.Sample.Core;


public class MainViewModel : INotifyPropertyChanged
{
    private string _text;
    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }

    public ICommand ButtonCommand { get; }


    public MainViewModel()
    {
        ButtonCommand = new ButtonCommand(this);
    }
    

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

public class ButtonCommand : ICommand
{
    private int i;
    
    private readonly MainViewModel _mainViewModel;

    public ButtonCommand(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _mainViewModel.Text = (i++).ToString();
    }

    public event EventHandler? CanExecuteChanged;
}