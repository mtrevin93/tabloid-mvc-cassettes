using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentDetailsViewModel
    {
        public Comment Comment { get; set; }
        public int CurrentUserId { get; set; }
    }
}
