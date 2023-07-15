namespace MauiChat.Views;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ChatPage : ContentPage
{
	public ChatPage()
	{
		InitializeComponent();
	}

    public string UserId
    {
        set { LoadMessages(value); }
    }

    private void LoadMessages(string userId)
    {

    }
}