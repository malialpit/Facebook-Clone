using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facebook_Clone.Models.Data
{
        
        [Table("posts")]
        public class PostDto
        {
            [Key]
            public int user_Id { get; set; }
            public int Post_Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Contents { get; set; }
            public string Image { get; set; }
          
        }
    
}