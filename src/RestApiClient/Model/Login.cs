using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class Login
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Only allowed username with A-Z, a-z and 0-9")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
