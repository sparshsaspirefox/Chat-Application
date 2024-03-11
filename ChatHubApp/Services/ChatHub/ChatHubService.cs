using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.ChatHub
{
    public class ChatHubService : IChatHubService
    {
        static string baseUrl = "https://7mwfwf0g-7074.inc1.devtunnels.ms";

        string userToken = string.Empty;
        public HubConnection _hubConnection;
        public async Task<HubConnection> CreateHubConnection()
        {
            if ( _hubConnection == null ||  _hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    userToken = Preferences.Get("Token", null);
                    _hubConnection = new HubConnectionBuilder()
                        .WithUrl($"{baseUrl}/chathub?access_token=" + userToken)
                        .WithAutomaticReconnect()
                        .Build();

                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    // Handle connection start failure
                    Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                    // You may want to add further error handling or logging here
                }

                //intialize for puch notification
                //_hubConnection.On<MessageViewModel>("SendNotification", (message) =>
                //{
                //    string description = string.Empty;
                //    if (message.ContentType == MessageType.Written.ToString())
                //    {
                //        description = message.Content;
                //    }
                //    else if (message.ContentType == MessageType.Image.ToString())
                //    {
                //        description = "New Image";
                //    }
                //    else
                //    {
                //        description = "New document";
                //    }
                //    Random rnd = new Random();
                //    var request = new NotificationRequest
                //    {
                //        NotificationId = rnd.Next(999999),
                //        Title = message.SenderName,
                //        Subtitle = "New Message",
                //        Description = description,
                //        BadgeNumber = 2,
                //        Schedule = new NotificationRequestSchedule
                //        {
                //            NotifyTime = DateTime.Now
                //        }
                //    };
                //    LocalNotificationCenter.Current.Show(request);
                //});
            }
            return _hubConnection;
        }

        public async Task DisconnectConnection()
        {
            if (_hubConnection != null || _hubConnection.State == HubConnectionState.Connected || _hubConnection.State == HubConnectionState.Connecting)
            {
                await _hubConnection.DisposeAsync();
              
            }
        }
    }
}
