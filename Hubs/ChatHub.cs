using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using web_chat_api.Models;
namespace web_chat_api.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<Guid, string> Members;
        public static Dictionary<Guid, List<Guid>> Rooms;
        public static Dictionary<Guid, List<Msg>> Msgs;
        public ChatHub()
        {
            if (Rooms == null)
            {
                Rooms = new Dictionary<Guid, List<Guid>>();
            }
            if (Members == null)
            {
                Members = new Dictionary<Guid, string>();
            }
            if (Msgs == null)
            {
                Msgs = new Dictionary<Guid, List<Msg>>();
            }

        }
        public async Task SendMsg(Guid userId, Guid roomId, Msg message)
        {
            lock (Msgs)
            {

                if (!Msgs.ContainsKey(roomId))
                {
                    Msgs.Add(roomId, new List<Msg>());
                }
                Msgs[roomId].Add(message);
            }
            lock (Rooms)
            {
                if (!Rooms.ContainsKey(roomId))
                {
                    Rooms.Add(roomId, new List<Guid>());
                }
                Rooms[roomId].Add(userId);
            }
            lock (Members)
            {
                Members[userId] = Context.ConnectionId;
            }
            var cnxs = new List<string>();
            foreach (var m in Members)
            {
                if (Rooms[roomId].Contains(m.Key))
                {
                    cnxs.Add(m.Value);
                }
            }
            await Clients.Clients(cnxs).SendAsync("Msg", roomId, message);

        }

        public async Task LoadAllMsgs(Guid roomId)
        {
            if (Msgs.ContainsKey(roomId) && Msgs[roomId] != null && Msgs[roomId].Count > 0)
            {
                await Clients.Caller.SendAsync("Msgs", roomId, Msgs[roomId]);
            }
        }
        public void AddMember(Guid userId)
        {
            lock (Members)
            {
                Members[userId] = Context.ConnectionId;
            }

        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}