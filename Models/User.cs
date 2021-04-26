using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
namespace web_chat_api.Models{
    public class User{

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        [JsonIgnore]
        public List<UserRoom> UserRooms { get; set; }
        [NotMapped]
        public List<Room> Rooms { get;set; }
    }
}