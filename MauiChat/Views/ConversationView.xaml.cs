namespace MauiChat.Views;

public partial class ConversationView : ContentPage
{
    public ConversationView(ConversationViewModel conversationViewModel)
    {
        InitializeComponent();

        base.BindingContext = conversationViewModel;

        MessagingCenter.Subscribe<AppShell, string>(this, "Conversation", (sender, message) =>
        {
            conversationViewModel.ReceiveSingleMessageShowConversation(message);
        });
    }

    private async void conversations_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var conversation = (Models.Conversation)e.CurrentSelection[0];

            await Shell.Current.GoToAsync($"{nameof(ChatView)}?{nameof(ChatView.UserId)}={conversation.UserId}");

            // Unselect the UI
            //conversations.SelectedItem = null;
        }
    }
}