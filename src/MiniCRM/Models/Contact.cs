using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCRM.Models
{
    public class Contact
    {
        public int ID { get; set; }
        [Display(Name = "First Name")]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(25, MinimumLength = 2)]
        public string LastName { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
