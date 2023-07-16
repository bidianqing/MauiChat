namespace MauiChat.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string buttonLabel);
    }
}
