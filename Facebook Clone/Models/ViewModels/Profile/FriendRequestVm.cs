using Facebook_Clone.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facebook_Clone.Models.ViewModels.Profile
{
    public class FriendRequestVm
    {
        public FriendRequestVm()
        {

        }

        public FriendRequestVm(FriendDto row)
        {
            User1 = row.User1;
            User2 = row.User2;
            Active = row.Active;
        }

        public int User1 { get; set; }
        public int User2 { get; set; }
        public bool Active { get; set; }
    }
}