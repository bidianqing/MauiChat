using MauiChat.Views;

namespace MauiChat;

public partial class App : Application
{
    private readonly IRequestService _requestService;
    public App(IRequestService requestService, LoginViewModel loginViewModel)
    {
        _requestService = requestService;

        string token = "";
        var loginResult = Task.Run(async () =>
        {
            return await _requestService.GetAsync<LoginResult>(Urls.RefreshToken);
        }).Result;

        if (loginResult != null && !string.IsNullOrWhiteSpace(loginResult.Token))
        {
            token = loginResult.Token;
            Preferences.Default.Set("jwt_token", loginResult.Token);
        }
        else
        {
            Preferences.Default.Set("jwt_token", "");
        }

        InitializeComponent();

        if (string.IsNullOrWhiteSpace(token))
        {
            MainPage = new LoginView(loginViewModel);
            return;
        }

        MainPage = new AppShell();
    }
}
