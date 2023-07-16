using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiChat.ViewModels
{
    public class ConversationsViewModel : ObservableObject
    {
        private readonly IRepositoryService _repositoryService;
        public ObservableCollection<ConversationViewModel> Conversations { get; }

        public ConversationsViewModel(IRepositoryService repositoryService)
        {
            Conversations = new ObservableCollection<ConversationViewModel>();
            _repositoryService = repositoryService;
            MessagingCenter.Subscribe<AppShell, string>(this, "Conversation", (sender, message) =>
            {
                this.ReceiveSingleMessageShowConversation(message);
            });
            
            LoadConversations();

            //var appShell = App.Current.MainPage as AppShell;
            //Task.Run(appShell.StartHubConnection);
        }

        private async void LoadConversations()
        {
            string sql = "select UserId,LastMessage,Date from conversation";
            var conversations = await _repositoryService.QueryAsync<ConversationViewModel>(sql);
            foreach (var item in conversations)
            {
                this.Conversations.Add(item);
            }
        }

        public async void ReceiveSingleMessageShowConversation(string message)
        {
            Trace.WriteLine(message);
            var obj = JObject.Parse(message);

            string fromUserId = obj["fromUserId"].ToString();
            string lastMessage = obj["message"].ToString();

            var conversationViewModel = Conversations.FirstOrDefault(u => u.UserId == fromUserId);
            if (conversationViewModel == null)
            {
                var entity = new Conversation
                {
                    UserId = fromUserId,
                    LastMessage = lastMessage,
                    Date = DateTime.Now
                };
                await _repositoryService.InsertAsync(entity);

                var viewModel = new ConversationViewModel
                {
                    UserId = fromUserId,
                    LastMessage = lastMessage,
                    Date = DateTime.Now
                };

                Conversations.Add(viewModel);
            }
            else
            {
                conversationViewModel.LastMessage = lastMessage;
                conversationViewModel.Date = DateTime.Now;
                //conversationViewModel.Reload();
                Conversations.Move(Conversations.IndexOf(conversationViewModel), 0);
            }


        }
    }
}
