using MauiChat.Views;

namespace MauiChat;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "mauichat.db");
        var connection = new SQLiteConnection(databasePath);
        var tableTypes = new[]
        {
            typeof(Conversation)
        };
        connection.CreateTables(CreateFlags.None, tableTypes);

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        var mauiApp = builder.Build();

        return mauiApp;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "mauichat.db");
        builder.Services.AddSingleton<IRepositoryService, RepositoryService>(sp => ActivatorUtilities.CreateInstance<RepositoryService>(sp, databasePath));
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IRequestService, RequestService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<ConversationViewModel>();

        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<ConversationView>();

        return builder;
    }
}
