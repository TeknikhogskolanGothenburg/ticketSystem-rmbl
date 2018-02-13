using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class User
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only allowed username with A-Z, a-z and 0-9")]
        public string Username { get; set; }
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression("(?=.*[A-Z])(?=.*[^\\w\\d])(?=.*[\\d])(?=.*[a-z])", ErrorMessage = "A password need minimum one of each A-Z, a-z, 0-9 and special character")]
        public string Password { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Email { get; set; }
        [Range(1,3)]
        public Int16 Grade { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Address { get; set; }
        [StringLength(25)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string City { get; set; }
    }
}
