using Facebook_Clone.Models.Data;
using Facebook_Clone.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Facebook_Clone.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            // Check if user is logged in
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return Redirect("~/" + username);

            // Return view
            return View();
        }

        // POST: Account/CreateAccount
        [HttpPost]
        public ActionResult CreateAccount(UserVm model, HttpPostedFileBase file)
        {
            // Initialise the database
            Fb db = new Fb();

            // Check model state
            if (!ModelState.IsValid)
                return View("Index", model);

            // Check if username is available
            if (db.users.Any(x => x.Username.Equals(model.Username))) {
                ModelState.AddModelError("", "Username " + model.Username + " is taken");
                model.Username = "";

                return View("Index", model);
            }

            // Create user DTO
            UserDto userDto = new UserDto()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };

            // Add to DTO
            db.users.Add(userDto);

            // Save changes
            db.SaveChanges();

            // Get last inserted id
            int userId = userDto.Id;

            // Log the user in
            FormsAuthentication.SetAuthCookie(model.Username, false);

            // Set upload directory
            var uploadDir = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));

            // Check if file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();

                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/gif" && ext != "image/png")
                {
                    ModelState.AddModelError("", "The image was not uploaded - invalid image");
                    return View("Index", model);
                }
            
                // Set image name
                string imageName = userId + ".jpg";

                // Set image path
                var path = (string.Format("{0}\\{1}", uploadDir, imageName));

                // Save image to the server
                file.SaveAs(path);
            }

            // Redirect
            return Redirect("~/" + model.Username);
        }

        // GET: Username/{username}
        [Authorize]
        public ActionResult Username(string username = "")
        {
            // Initialise database
            Fb db = new Fb();

            // Check if user exists
            if (!db.users.Any(x => x.Username.Equals(username)))
                return Redirect("~/");

            // Viewbag username
            ViewBag.username = username;

            // Get user's username
            string user = User.Identity.Name;

            // Viewbag user's full name
            UserDto userDto = db.users.Where(x => x.Username.Equals(user)).FirstOrDefault();
            ViewBag.FullName = userDto.FirstName + " " + userDto.LastName;

            // Get user's id
            int userId = userDto.Id;

            // Get viewer full name
            UserDto userDto2 = db.users.Where(x => x.Username.Equals(username)).FirstOrDefault();
            ViewBag.ViewingFullName = userDto2.FirstName + " " + userDto2.LastName;

            // Viewbag visited profile's image
            ViewBag.ProfileImage = userDto2.Id + ".jpg";

            // Viewbag user type
            string userType = "guest";

            if (username.Equals(user))
                userType = "owner";

            ViewBag.UserType = userType;

            // Check if they are friends
            if (userType == "guest")
            {
                UserDto u1 = db.users.Where(x => x.Username.Equals(user)).FirstOrDefault();
                int myId = u1.Id;

                UserDto u2 = db.users.Where(x => x.Username.Equals(username)).FirstOrDefault();
                int viewingProfileId = u2.Id;

                FriendDto f1 = db.friends.Where(x => x.User1 == myId && x.User2 == viewingProfileId).FirstOrDefault();
                FriendDto f2 = db.friends.Where(x => x.User2 == myId && x.User1 == viewingProfileId).FirstOrDefault();

                if (f1 == null && f2 == null)
                {
                    ViewBag.NotFriends = "True";
                }
                if (f1 != null)
                {
                    if (!f1.Active)
                    {
                        ViewBag.NotFriends = "Pending";
                    }
                }
                if (f2 != null)
                {
                    if (!f2.Active)
                    {
                        ViewBag.NotFriends = "Pending";
                    }
                }
            }

            // Viewbag friend request count
            var friendcount = db.friends.Count(x => x.User2 == userId && x.Active == false);

            if (friendcount > 0)
            {
                ViewBag.FrCount = friendcount;
            }

            // Viewbag friend count
            UserDto uDto = db.users.Where(x => x.Username.Equals(username)).FirstOrDefault();
            int usernameId = uDto.Id;

            var friendCount2 = db.friends.Count(x => x.User2 == usernameId && x.Active == true || x.User1 == usernameId && x.Active == true);

            ViewBag.FCount = friendCount2;

            return View();
        }

        // GET: Account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            // Log out
            FormsAuthentication.SignOut();

            // Redirect
            return Redirect("~/");
        }

        public ActionResult LoginPartial()
        {
            return PartialView();
        }

        // POST: Account/Login
        [HttpPost]
        public string Login(string username, string password)
        {
            // Initialise database
            Fb db = new Fb();

            // Check if user exists
            if (db.users.Any(x => x.Username.Equals(username) && x.Password.Equals(password)))
            {
                // Log the user in
                FormsAuthentication.SetAuthCookie(username, false);
                return "ok";
            } else
            {
                return "problem";
            }
        }
    }
}