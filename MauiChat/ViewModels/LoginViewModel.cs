namespace MauiChat.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IRequestService _requestService;

        [ObservableProperty]
        private string _phone;

        public LoginViewModel(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [RelayCommand]
        private async Task Login()
        {
            string phone = this.Phone;
            var json = new JsonObject
            {
                ["phone"] = phone,
                ["code"] = "0000"
            }.ToJsonString();

            var result = await _requestService.PostAsync<LoginResult>(Urls.Login, json);
            if (result != null)
            {
                Preferences.Default.Set("jwt_token", result.Token);
                var appShell = new AppShell();
                App.Current.MainPage = appShell;
                await appShell.StartHubConnection();
            }
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }

        public int Flag { get; set; }
    }
}
