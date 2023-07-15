using MauiChat.Models;
using MauiChat.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotification;
using SQLite;

namespace MauiChat;

public partial class App : Application
{
    private readonly HubConnection _connection;
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        var db = new SQLiteConnection(Constants.DatabasePath);
        db.CreateTable<Conversation>();

        _connection = new HubConnectionBuilder()
                .WithUrl($"https://bidianqing.natappvip.cc/chathub", options =>
                {
                    options.Headers.Add("Platform", "app");
                    options.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMTEyMTc0MDYzNDczOTM0MzM2MCIsIm5iZiI6MTY4NzUxNDIwOSwiZXhwIjoxNjkwMDU2MDAwLCJpc3MiOiJjdXBpZGNvbWVvcmciLCJhdWQiOiJjY3VzZXIifQ.HPhIXuiOi7Knb5o7E1kvnyowXK0dw6A2FyD5vX_6Exc");
                })
                .Build();

        _connection.On<string, string>("receiveSingleMessage", (fromUserId, message) =>
        {
            Console.WriteLine(message);

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
                    //Schedule = new NotificationRequestSchedule
                    //{
                    //    NotifyTime = DateTime.Now.AddSeconds(5),
                    //    NotifyRepeatInterval = TimeSpan.FromDays(1)
                    //}
                };
                LocalNotificationCenter.Current.Show(request);
            }

            var obj = new JObject
            {
                ["fromUserId"] = fromUserId,
                ["message"] = message
            };
            MessagingCenter.Send<App, string>(this, "Conversation", obj.ToString(Newtonsoft.Json.Formatting.None));
        });

        Task.Run(() =>
        {
            

            base.Dispatcher.Dispatch(async () => await _connection.StartAsync());
        });
    }

    
}
