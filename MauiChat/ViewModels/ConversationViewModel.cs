using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiChat.ViewModels
{
    public class ConversationViewModel : ObservableObject
    {
        private readonly IRepositoryService _repositoryService;
        public ObservableCollection<Conversation> Conversations { get; }

        public ConversationViewModel(IRepositoryService repositoryService)
        {
            Conversations = new ObservableCollection<Conversation>();
            _repositoryService = repositoryService;
        }

        public async void ReceiveSingleMessageShowConversation(string message)
        {
            Trace.WriteLine(message);
            var obj = JObject.Parse(message);

            string fromUserId = obj["fromUserId"].ToString();
            string lastMessage = obj["message"].ToString();

            var c = Conversations.FirstOrDefault(u => u.UserId == fromUserId);
            if (c == null)
            {
                var entity = new Conversation
                {
                    UserId = fromUserId,
                    LastMessage = lastMessage,
                    Date = DateTime.Now
                };

                await _repositoryService.InsertAsync(entity);
                Conversations.Add(entity);
            }
            else
            {
                c.LastMessage = lastMessage;
                c.Date = DateTime.Now;
            }
        }
    }
}
