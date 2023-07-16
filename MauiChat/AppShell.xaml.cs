using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotification;

namespace MauiChat;

public partial class AppShell : Shell
{
    private HubConnection _connection;
    public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ChatView), typeof(Views.ChatView));

        _connection = new HubConnectionBuilder()
               .WithUrl($"https://bidianqing.natappvip.cc/chathub", options =>
               {
                   string token = Preferences.Default.Get("jwt_token", "");
                   options.Headers["Authorization"] = $"Bearer {token}";
                   options.Headers["Platform"] = $"app";
               })
               .Build();

        _connection.On<string, string>("receiveSingleMessage", (fromUserId, message) =>
        {
            if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1000,
                    Title = message,
                    Subtitle = message,
                    Description = message,
                    BadgeNumber = 42,
                    Group = "MauiChat"
                };
                LocalNotificationCenter.Current.Show(request);
            }

            var obj = new JObject
            {
                ["fromUserId"] = fromUserId,
                ["message"] = message
            };
            MessagingCenter.Send<AppShell, string>(this, "Conversation", obj.ToString(Newtonsoft.Json.Formatting.None));
        });

        Task.Run(StartHubConnection);
    }


    public async Task StartHubConnection()
    {
        await _connection.StartAsync();
    }
}
