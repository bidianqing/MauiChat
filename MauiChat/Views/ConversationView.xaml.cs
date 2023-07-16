namespace MauiChat.Views;

public partial class ConversationView : ContentPage
{
    public ConversationView(ConversationsViewModel conversationViewModel)
    {
        InitializeComponent();

        base.BindingContext = conversationViewModel;
    }

    private async void conversations_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var conversationViewModel = (ConversationViewModel)e.CurrentSelection[0];

            await Shell.Current.GoToAsync($"{nameof(ChatView)}?{nameof(ChatView.UserId)}={conversationViewModel.UserId}");

            // Unselect the UI
            //conversations.SelectedItem = null;
        }
    }
}