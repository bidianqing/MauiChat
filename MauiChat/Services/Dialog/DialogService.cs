namespace MauiChat.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string title, string message, string buttonLabel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }
    }
}
