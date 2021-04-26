using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
namespace web_chat_api.Models{
    public class Room{
        public Guid RoomId { get; set; }
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public List<UserRoom> UserRooms { get; set; }
        [NotMapped]
        public List<Msg> AllMsgs { get; set; }
        [NotMapped]
        public List<Msg> UnreadMsgs { get; set; }
        [NotMapped]
        public List<User> Members { get;set; }
    }
}