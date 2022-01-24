using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Entities
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300,MinimumLength =4)]
        public string Name { get; set; }
        [Required]
        public string ImagePath { get; set; }
       
        public int Discount { get; set; }
        [Column(TypeName = "money")]
        [Required]
        public decimal Price { get; set; }
       
        public string details { get; set; }
        public bool Availabitlty { get; set; }

        
    }
}
