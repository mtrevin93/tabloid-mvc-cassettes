﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class SubscribedPostListViewModel
    {
        public List<Post> Posts { get; set; }
        public UserProfile User { get; set; }
    }
}
