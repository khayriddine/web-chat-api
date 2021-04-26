using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using web_chat_api.Data;
using web_chat_api.Models;

namespace web_chat_api.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase{
        private readonly ChatContext _context;
        public UsersController(ChatContext context)
        {
            _context = context;
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(Guid id)
        {
            var user =  _context.Users
            .Include(u => u.UserRooms)
            .ThenInclude(ur => ur.Room)
            .AsNoTracking().FirstOrDefault(u => u.UserId == id);
            if(user == null){
                return NotFound();
            }
            user.Rooms = new List<Room>();

            user.Rooms.AddRange(user.UserRooms?.Select(ur => {
                var room = ur.Room;
                room?.UserRooms?.Clear();
                return room;
            }).ToList());
            user.UserRooms.Clear();
            foreach(var r in user.Rooms){
                var urs = _context.UserRooms.Where(ur => ur.RoomId == r.RoomId).Select(ur => ur.User).ToList();
                r.Members = new List<User>();
                r.Members.AddRange(urs);
            }
            return Ok(user);
        }

    }
}