using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public PostTag PostTag { get; set; }

        public int PostId { get; set; }

        public List<Tag> Tag { get; set; }

        public List<int> TagsSelected { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}
