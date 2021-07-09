using Facebook_Clone.Models.Data;
using Facebook_Clone.Models.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facebook_Clone.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        // GET: Profile/Search
        public JsonResult Search(string query)
        {
            // Initialise database
            Fb db = new Fb();

            // Create list 
            List<SearchVm> usernames = db.users.Where(x => x.Username.Contains(query) && x.Username != User.Identity.Name).ToArray().Select(x => new SearchVm(x)).ToList();

            // Return JSON
            return Json(usernames);
        }

        // POST: Profile/AddFriend
        [HttpPost]
        public void AddFriend(string friend)
        {
            // Initialise database
            Fb db = new Fb();

            // Get user's id
            UserDto userDto = db.users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Get friend to be's id
            UserDto userDto2 = db.users.Where(x => x.Username.Equals(friend)).FirstOrDefault();
            int friendId = userDto2.Id;

            FriendDto friendDto = new FriendDto();

            friendDto.User1 = userId;
            friendDto.User2 = friendId;
            friendDto.Active = false;

            db.friends.Add(friendDto);

            db.SaveChanges();
        }

        // POST: Profile/DisplayFriendRequests
        [HttpPost]
        public JsonResult DisplayFriendRequests(string friend)
        {
            // Initialise database
            Fb db = new Fb();

            // Get user id
            UserDto userDto = db.users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Create list of friends
            List<FriendRequestVm> list = db.friends.Where(x => x.User2 == userId && x.Active == false).ToArray().Select(x => new FriendRequestVm(x)).ToList();

            // Init list of users
            List<UserDto> users = new List<UserDto>();

            foreach (var item in list)
            {
                var user = db.users.Where(x => x.Id == item.User1).FirstOrDefault();

                users.Add(user);
            }

            // Return json
            return Json(users);
        }

        // POST: Profile/AcceptFriendRequest
        [HttpPost]
        public void AcceptFriendRequest(int friendId)
        {
            // Initialise database
            Fb db = new Fb();

            // Get user id
            UserDto userDto = db.users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Make friends
            FriendDto friendDto = db.friends.Where(x => x.User1 == friendId && x.User2 == userId).FirstOrDefault();

            friendDto.Active = true;

            db.SaveChanges();
        }

        // POST: Profile/DeclineFriendRequest
        [HttpPost]
        public void DeclineFriendRequest(int friendId)
        {
            // Initialise database
            Fb db = new Fb();

            // Get user id
            UserDto userDto = db.users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDto.Id;

            // Remove friend request
            FriendDto friendDto = db.friends.Where(x => x.User1 == friendId && x.User2 == userId).FirstOrDefault();

            db.friends.Remove(friendDto);

            db.SaveChanges();
        }
        
        public ActionResult AddPost()
        {

            Fb db = new Fb();

            UserDto userDto = db.users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            int user_Id = userDto.Id;

           List <PostDto> postDto = db.posts.ToList();

            return View();



        }
    }
}