using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class Home
    {
        private HubConnection? hubConnection;
        private HubConnection _hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;
        string baseUrl;

        protected override async Task OnInitializedAsync()
        {
            Connection();
          
        }

        private void Connection()
        {
            baseUrl = "https://5hgrdskg-7074.inc1.devtunnels.ms";
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                baseUrl = "https://5hgrdskg-7074.inc1.devtunnels.ms";
            }

            _hubConnection = new HubConnectionBuilder().WithUrl($"{baseUrl}/chathub").Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                InvokeAsync(StateHasChanged);
                //MessageList.Add(new MessageList() { User = user, Message = message });
                //this.InvokeAsync(() => this.StateHasChanged());

            });

            Task.Run(async () =>
            {
                await _hubConnection.StartAsync();
            });
        }

        private async Task Send()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.SendAsync("SendMessage", userInput, messageInput);
            }
        }

        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
