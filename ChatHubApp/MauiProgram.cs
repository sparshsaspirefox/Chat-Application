using ChatHubApp.HttpApiManager;
using ChatHubApp.Services.Account;
using ChatHubApp.Services.Audio;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.FileUpload;
using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Group;
using ChatHubApp.Services.Message;
using ChatHubApp.Services.Notification;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using Radzen;
using Tewr.Blazor.FileReader;
using INotificationService = ChatHubApp.Services.Notification.INotificationService;
using NotificationService = ChatHubApp.Services.Notification.NotificationService;

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
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddRadzenComponents();

            builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IApiManager,ApiManager>();
         
            builder.Services.AddTransient<IAccountService,AccountService>();
            builder.Services.AddTransient<IMessageService, MessageService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<IFriendService, FriendService>();
            builder.Services.AddTransient<IGroupService, GroupService>();
            builder.Services.AddSingleton<IChatHubService, ChatHubService>();
            builder.Services.AddSingleton<IFileUploadService, FileUploadService>();
            builder.Services.AddSingleton<IAudioService, AudioService>();


            //for audio
            builder.Services.AddSingleton(AudioManager.Current);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
