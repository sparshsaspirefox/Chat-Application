using Microsoft.AspNetCore.SignalR.Client;
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
                userToken = Preferences.Get("Token", null);
                _hubConnection = new HubConnectionBuilder().WithUrl($"{baseUrl}/chathub?access_token=" + userToken).WithAutomaticReconnect().Build();
                await _hubConnection.StartAsync();
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
