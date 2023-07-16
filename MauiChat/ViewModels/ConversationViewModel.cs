namespace MauiChat.ViewModels
{
    public partial class ConversationViewModel : ObservableObject
    {
        public string UserId { get; set; }

        [ObservableProperty]
        private string _lastMessage;

        [ObservableProperty]
        private DateTime _date;

        public ConversationViewModel()
        {

        }


        public void Reload()
        {
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(LastMessage));
            OnPropertyChanged(nameof(Date));
        }
    }
}
