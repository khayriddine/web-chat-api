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
    public class RoomsController : ControllerBase{
        private readonly ChatContext _context;
        public RoomsController(ChatContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<Room> GetRoom(Guid id){
            var room =  _context.Rooms
            .Include(r => r.UserRooms)
            .AsNoTracking().FirstOrDefault(r => r.RoomId == id);
            if(room == null){
                return NotFound();
            }
            return Ok(room);
        }
    }
}