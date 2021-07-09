using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using Facebook_Clone.Models.Data;

namespace Facebook_Clone
{
    [HubName("echo")]
    public class EchoHub : Hub
    {
        public void Hello(string message)
        {
            Trace.WriteLine(message);
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void Notify(string friend)
        {
            // Initialise database
            Fb db = new Fb();

            // Get friend's id
            UserDto userDto = db.users.Where(x => x.Username.Equals(friend)).FirstOrDefault();
            int friendId = userDto.Id;

            // Get friend count
            var frcount = db.friends.Count(x => x.User2 == friendId && x.Active == false);

            // Set clients
            var clients = Clients.Others;

            // Call js function
            clients.frnotify(friend, frcount);
        }

        public void GetFrCount()
        {
            // Initialise database
            Fb db = new Fb();

            // Get user id
            UserDto userDto = db.users.Where(x => x.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Get friend count
            var friendCount = db.friends.Count(x => x.User2 == userId && x.Active == false);

            // Set clients
            var clients = Clients.Caller;

            // Call js function
            clients.frcount(Context.User.Identity.Name, friendCount);
        }

        public void GetFCount(int friendId)
        {
            // Initialise database
            Fb db = new Fb();

            // Get user id
            UserDto userDto = db.users.Where(x => x.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Get friend count for user
            var friendCount1 = db.friends.Count(x => x.User2 == userId && x.Active == true || x.User1 == userId && x.Active == true);

            // Get user2 username
            UserDto userDto2 = db.users.Where(x => x.Id == friendId).FirstOrDefault();
            string username = userDto2.Username; // logged in user

            // Get friend count for user
            var friendCount2 = db.friends.Count(x => x.User2 == friendId && x.Active == true || x.User1 == friendId && x.Active == true);

            // Set clients
            var clients = Clients.All;

            // Call js function
            clients.fcount(Context.User.Identity.Name, username, friendCount1, friendCount2);
        }
    }
}