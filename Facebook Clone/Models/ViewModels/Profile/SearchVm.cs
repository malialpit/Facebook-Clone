using Facebook_Clone.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facebook_Clone.Models.ViewModels.Profile
{
    public class SearchVm
    {
        public SearchVm()
        {

        }

        public SearchVm(UserDto row)
        {
            UserId = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Username = row.Username;
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}