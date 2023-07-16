namespace MauiChat.Views;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class ChatView : ContentPage
{
	public ChatView()
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