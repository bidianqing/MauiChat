using SQLite;
using System.Collections.ObjectModel;

namespace MauiChat.Models
{
    [Table("conversation")]
    public class Conversation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string LastMessage { get; set; }

        public DateTime Date { get; set; }
    }

    public class AllConversation
    {
        public ObservableCollection<Conversation> Conversations { get; set; } = new ObservableCollection<Conversation>();

        public AllConversation() =>
            LoadConversation();

        private void LoadConversation()
        {
            this.Conversations.Clear();

            var db = new SQLiteConnection(Constants.DatabasePath);
            var conversations = db.Table<Conversation>().ToList();
            foreach (var item in conversations)
            {
                Conversations.Add(item);
            }
        }

        private void DeleteCommand(Conversation conversation)
        {

        }
    }
}
