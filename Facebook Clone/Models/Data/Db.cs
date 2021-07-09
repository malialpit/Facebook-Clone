using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Facebook_Clone.Models.Data
{
    public class Fb : DbContext
    {
        public DbSet<UserDto> users { get; set; }
        public DbSet<FriendDto> friends { get; set; }
        public DbSet<PostDto> posts { get; set; }
    }
}