using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatHubApi.Hubs
{
    public class ConnectedUser
    {
        public string UserId { get; set; }
        public string ConnnectionId { get; set; }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatHub:Hub
    {

        static List<ConnectedUser> ConnectedUsers = new List<ConnectedUser>();

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);


            var existingUser = ConnectedUsers.FirstOrDefault(x => x.UserId == userId);
            var indexExistingUser = ConnectedUsers.IndexOf(existingUser);

            ConnectedUser user = new ConnectedUser
            {
                UserId = userId,
                ConnnectionId = Context.ConnectionId
            };

            if (!ConnectedUsers.Contains(existingUser))
            {
                ConnectedUsers.Add(user);
            }
            else
            {
                ConnectedUsers[indexExistingUser] = user;
            }
            await GetAllActiveUsers();
            
        }

        public async Task GetAllActiveUsers()
        {
            List<string> activeUsers = new List<string>();
            foreach (var user in ConnectedUsers)
            {
                activeUsers.Add(user.UserId);
            }
           
            await Clients.All.SendAsync("ActiveUsers", activeUsers);
            await Clients.All.SendAsync("UpdateActiveUsers");
        }

        public async Task SendMessage(MessageViewModel messageViewModel)
        {
            await Clients.All.SendAsync("ReceiveMessage",messageViewModel);
        }

        public async Task UpdateMessageCount(string SenderId,string ReceiverId)
        {
            try
            {
                ConnectedUser ReceiverConn = ConnectedUsers.FirstOrDefault(u => u.UserId == ReceiverId);
                if (ReceiverConn != null)
                {
                    string ReceiverConnId = ReceiverConn.ConnnectionId;
                    await Clients.Client(ReceiverConnId).SendAsync("UpdateMessageCount", SenderId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateSeenMessages(string SenderId, string ReceiverId)
        {
            try
            {
                ConnectedUser ReceiverConn = ConnectedUsers.FirstOrDefault(u => u.UserId == ReceiverId);
                if (ReceiverConn != null)
                {
                    string ReceiverConnId = ReceiverConn.ConnnectionId;
                    await Clients.Client(ReceiverConnId).SendAsync("UpdateSeenMessages", SenderId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // await DisconnectOnLogout();

        }

        public async Task DisconnectOnLogout()
        {
            try
            {
                string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var existingUser = ConnectedUsers.FirstOrDefault(x => x.UserId == userId);

             
                if (existingUser != null)
                {
                    ConnectedUsers.Remove(existingUser);
                }
                await GetAllActiveUsers();
            }
            catch(Exception ex)
            {

            }
            

        }
    }
}
