using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public int Price { get; set; }
        public int Profit { get; set; }
        public decimal Rating { get; set; }

    }
}
