using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Areas.Admin.Models
{
    public class UserEdit
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        public string Pincode { get; set; }
        public string PhoneNo { get; set; }

    }
}
