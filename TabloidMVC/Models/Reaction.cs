using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public string ImageLocation { get; set; }
        public string Name { get; set; }
        public int TimesUsed { get; set; }
    }
}
