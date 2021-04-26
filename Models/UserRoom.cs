using System;

namespace web_chat_api.Models{
    public class UserRoom{
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }

        public User User { get; set; }
        public Room Room { get; set; }

    }
}