using MauiChat.Models;
using Newtonsoft.Json.Linq;
using SQLite;
using System.Diagnostics;

namespace MauiChat.Views;

public partial class ConversationPage : ContentPage
{
    public ConversationPage()
    {
        InitializeComponent();

        base.BindingContext = new AllConversation();

        MessagingCenter.Subscribe<App, string>(this, "Conversation", (sender, message) =>
        {
            Trace.WriteLine(message);
            var obj = JObject.Parse(message);

            string fromUserId = obj["fromUserId"].ToString();
            string lastMessage = obj["message"].ToString();

            var allConversation = base.BindingContext as AllConversation;

            var c = allConversation.Conversations.FirstOrDefault(u => u.UserId == fromUserId);
            if (c == null)
            {
                var entity = new Conversation
                {
                    UserId = fromUserId,
                    LastMessage = lastMessage,
                    Date = DateTime.Now
                };
                var db = new SQLiteConnection(Constants.DatabasePath);
                db.Insert(entity);
                allConversation.Conversations.Add(new Conversation
                {
                    UserId = fromUserId,
                    LastMessage = lastMessage,
                    Date = DateTime.Now
                });
            }
            else
            {
                c.LastMessage = lastMessage;
                c.Date = DateTime.Now;
            }
        });
    }

    private async void conversations_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var conversation = (Models.Conversation)e.CurrentSelection[0];

            await Shell.Current.GoToAsync($"{nameof(ChatPage)}?{nameof(ChatPage.UserId)}={conversation.UserId}");

            // Unselect the UI
            conversations.SelectedItem = null;
        }
    }
}