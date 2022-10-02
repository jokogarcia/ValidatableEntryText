namespace Sample;

public partial class MVVMPage : ContentPage
{
	public MVVMPage()
	{
		InitializeComponent();
		BindingContext = new ViewModel.MVVMPageViewModel();
	}
}