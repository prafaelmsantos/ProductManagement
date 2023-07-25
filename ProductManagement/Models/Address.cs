using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        public string Street { get; set; }

        [Required, Range(1, 9999)]
        public int Number { get; set; }

        [Required, Range(1111111, 9999999)]
        public int PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public bool Selected { get; set; }

        [NotMapped]
        public string CompleteAddress
        {
            get
            {
                return $"{Street}, Nº{Number} {PostalCode}, {City}, {Country}";
            }
        }
    }
}
