using Data.Models;
using ChatHubApi.Hubs.HubModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatHubApi.Hubs
{
   
    

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
            try
            {
                ConnectedUser ReceiverConn = ConnectedUsers.FirstOrDefault(u => u.UserId == messageViewModel.ReceiverId);
                if (ReceiverConn != null)
                {
                    string ReceiverConnId = ReceiverConn.ConnnectionId;
                    await Clients.Client(ReceiverConnId).SendAsync("ReceiveMessage", messageViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           // await Clients.All.SendAsync("ReceiveMessage",messageViewModel);
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

        public async Task ShowTyping(string userId, string friendId)
        {
            ConnectedChat connectedChat = ConnectedChats.FirstOrDefault(p => p.UserId == friendId && p.FriendId == userId);
            ConnectedUser connectedUser = ConnectedUsers.FirstOrDefault(u => u.UserId == friendId);
            if (connectedChat != null)
            {
                await Clients.Client(connectedUser.ConnnectionId).SendAsync("ShowTyping", userId);
            }
        }

        public async Task HideTyping(string userId, string friendId)
        {
            ConnectedChat connectedChat = ConnectedChats.FirstOrDefault(p => p.UserId == friendId && p.FriendId == userId);
            ConnectedUser connectedUser = ConnectedUsers.FirstOrDefault(u => u.UserId == friendId);
            if (connectedChat != null)
            {
                await Clients.Client(connectedUser.ConnnectionId).SendAsync("HideTyping", userId);
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


        //connected chats

        static List<ConnectedChat> ConnectedChats = new List<ConnectedChat>();
        public async Task AddActiveChats(string userId,string friendId)
        {
            ConnectedChats.Add(new ConnectedChat() { UserId = userId,FriendId = friendId});
        }

        public async Task RemoveActiveChats(string userId,string friendId)
        {
            ConnectedChat connectedChat = ConnectedChats.FirstOrDefault(p=> p.UserId == userId && p.FriendId == friendId);
            if(connectedChat != null)
            {
                ConnectedChats.Remove(connectedChat);
            }
        }

        public async Task IsChatActive(string userId, string friendId)
        {
            //swap the ids beacuse we want to check did another person also open the chat?
            ConnectedChat connectedChat = ConnectedChats.FirstOrDefault(p => p.UserId == friendId && p.FriendId == userId);
            ConnectedUser connectedUser = ConnectedUsers.FirstOrDefault(u => u.UserId == userId);
            if (connectedChat != null)
            {
                await Clients.Client(connectedUser.ConnnectionId).SendAsync("ActiveChat",userId);
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
