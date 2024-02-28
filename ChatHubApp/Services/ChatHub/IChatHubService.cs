using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.ChatHub
{
    public interface IChatHubService
    {
        Task<HubConnection> CreateHubConnection();
    }
}
