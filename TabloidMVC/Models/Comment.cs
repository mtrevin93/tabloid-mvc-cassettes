using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Comment
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [DisplayName("Published")]
        [DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }
        public UserProfile Author { get; set; }
        public int PostId { get; set; }

    }
}
