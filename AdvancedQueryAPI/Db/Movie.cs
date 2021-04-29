using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Db
{
    public class Movie
    {        
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public int Price { get; set; }
        public int Profit { get; set; }
        public decimal Rating { get; set; }
    }
}
