namespace MauiChat.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
		InitializeComponent();

		base.BindingContext = loginViewModel;
    }
}