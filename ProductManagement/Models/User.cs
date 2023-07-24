using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }

        [Required]
        public int Phone { get; set; }

        [ReadOnly(true)]
        public DateTime? RegistrationDate { get; }
    }
}
