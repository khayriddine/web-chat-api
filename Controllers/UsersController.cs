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
        [HttpGet]
        public ActionResult<List<User>> GetUsers(){
            return _context.Users.ToList();
        }
        [HttpPost]
        [Route("auth")]
        public ActionResult<User> Authenticate(UserCredential uc){
             var user =  _context.Users
            .Include(u => u.UserRooms)
            .ThenInclude(ur => ur.Room)
            .AsNoTracking().FirstOrDefault(u => u.Name == uc.Name && u.Password == uc.Password);
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
        
        [HttpPost]
        [Route("register")]
        public ActionResult<User> Register(User user){
            if(_context.Users == null){
                return NotFound();
            }
            var us = _context.Users.FirstOrDefault(us => us.Name == user.Name);
            if(us != null)
            {
                return Problem("Choose different Name!");
            }
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            if(result.Entity != null)
                return Ok(user);
            else {
                return NotFound();
            }
            
        }
        [HttpPost]
        [Route("join/{userId}")]
        public ActionResult<Boolean> JoinRoom(Guid userId, RoomCredential rc){
            if(_context.Users == null ||_context.Rooms == null){
                return NotFound();
            }
            var room = _context.Rooms.AsNoTracking().FirstOrDefault(r => r.Name == rc.Name && r.Password == rc.Password);
            if(room != null){
                var result =_context.UserRooms.Add(new UserRoom{UserId = userId, RoomId = room.RoomId});
                _context.SaveChanges();
                if(result.Entity != null){
                    return Ok(true);
                }
                return Ok(false);
            }
            return Problem("Name or Password is incorrect !");
        }
        [HttpPut]
        [Route("leave/{userId}/{roomId}")]
        public ActionResult<Boolean> JoinRoom(Guid userId,Guid roomId){
            if(_context.Users == null ||_context.Rooms == null){
                return NotFound();
            }
            var room = _context.Rooms.AsNoTracking().FirstOrDefault(r => r.RoomId == roomId);
            if(room != null){
                if(room.AdminId == userId){
                    _context.UserRooms.RemoveRange(_context.UserRooms.Where(ur => ur.RoomId == roomId).ToList());
                    _context.SaveChanges();
                    _context.Rooms.Remove(room);
                    _context.SaveChanges();
                }
                else {
                    _context.UserRooms.RemoveRange(_context.UserRooms.Where(ur => ur.RoomId == roomId && ur.UserId == userId));
                }
                
                return Ok(true);
            }
            return Problem("Name or Password is incorrect !");
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