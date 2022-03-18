using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Message { get; set; }
    }
}
