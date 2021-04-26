using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_chat_api.Models;

namespace web_chat_api.Data{
    public static class DbInitializer{
        public static void Initialize(ChatContext context){

            //context.Database.EnsureCreated();
            if(context.Users.Any()){
                return;
            }
            User[] users = new User[]{
                new User{Name = "khayri", Password="123", Image="https://randomuser.me/api/portraits/thumb/lego/1.jpg"},
                new User{Name = "sameh", Password="123", Image="https://randomuser.me/api/portraits/thumb/lego/2.jpg"},
                new User{Name = "manel", Password="123", Image="https://randomuser.me/api/portraits/thumb/lego/3.jpg"},
            };

            foreach(var u in users){
                context.Users.Add(u);
            }
            context.SaveChanges();
            Room[] rooms = new Room[]{
                new Room{ Name = "My Room 1", Password ="", AdminId =users[0].UserId},
                new Room{ Name = "My Room 2", Password ="123", AdminId =users[1].UserId},
            };

            foreach(var r in rooms){
                context.Rooms.Add(r);
            }
            context.SaveChanges();

            UserRoom[] userRooms = new UserRoom[]{
                new UserRoom{ UserId = users[0].UserId, RoomId = rooms[0].RoomId },
                new UserRoom{ UserId = users[1].UserId, RoomId = rooms[0].RoomId },
                new UserRoom{ UserId = users[2].UserId, RoomId = rooms[0].RoomId },
                new UserRoom{ UserId = users[1].UserId, RoomId = rooms[1].RoomId },
                new UserRoom{ UserId = users[2].UserId, RoomId = rooms[1].RoomId },
            } ;
            foreach(var ur in userRooms){
                context.UserRooms.Add(ur);
            }
            context.SaveChanges();
            /*
            UserRoom[] userRooms = new UserRoom[]{
                new UserRoom{}
            }*/
        }
    }
}