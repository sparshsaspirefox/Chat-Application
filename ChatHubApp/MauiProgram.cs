using ChatHubApp.HttpApiManager;
using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Message;
using ChatHubApp.Services.Notification;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ChatHubApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IApiManager,ApiManager>();
         
            builder.Services.AddTransient<IAccountService,AccountService>();
            builder.Services.AddTransient<IMessageService, MessageService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<IFriendService, FriendService>();
            builder.Services.AddSingleton<IChatHubService, ChatHubService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
