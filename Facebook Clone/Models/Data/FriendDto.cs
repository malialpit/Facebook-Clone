using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook_Clone.Models.Data
{
    [Table("friends")]
    public class FriendDto
    {
        [Key]
        public int Id { get; set; }
        public int User1 { get; set; }
        public int User2 { get; set; }
        public bool Active { get; set; }

        [ForeignKey("User1")]
        public virtual UserDto Users1 { get; set; }
        [ForeignKey("User2")]
        public virtual UserDto Users2 { get; set; }
    }
}