﻿using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int isDeleted { get; set; }
    }
}