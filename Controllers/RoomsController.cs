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
        [HttpGet]
        public ActionResult<List<Room>> GetRooms(){
            if(_context.Rooms == null){
                return NotFound();
            }
            return _context.Rooms.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Room> GetRoom(Guid id){
            var room =  _context.Rooms
            .Include(r => r.UserRooms)
            .AsNoTracking().FirstOrDefault(r => r.RoomId == id);
            if(room == null){
                return NotFound();
            }
            room.UserRooms.Clear();
            return Ok(room);
        }
        [HttpPost]
        public ActionResult<Boolean> CreateRoom(Room room){
            if(_context.Rooms == null){
                return NotFound();
            }
            var ro = _context.Rooms.AsNoTracking().FirstOrDefault(r => r.Name == room.Name);
            if(ro != null){
                return Problem("Choose different Name!");
            }
            var result = _context.Rooms.Add(room);
            _context.SaveChanges();
            
            if(result.Entity != null){
                _context.UserRooms.Add(new UserRoom{UserId = room.AdminId, RoomId = room.RoomId});
                _context.SaveChanges();
                room.UserRooms.Clear();
                return Ok(true);
            }
            return Ok(false);
            
            //var usersController = new UsersController(_context);
            //return usersController.GetUser(room.AdminId); 
        }
    }
}