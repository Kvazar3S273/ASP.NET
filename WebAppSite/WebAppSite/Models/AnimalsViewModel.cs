using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSite.Models
{
    public class AnimalsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
