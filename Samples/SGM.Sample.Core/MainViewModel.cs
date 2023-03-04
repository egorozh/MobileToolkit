using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace SGM.Sample.Core;


public partial class MainViewModel : ObservableObject
{
    private int i;
    
    
    [ObservableProperty]
    private string _text;
    
    
    [RelayCommand]
    private void Click()
    {
        Text = (i++).ToString();
    }
}