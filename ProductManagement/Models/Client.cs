using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProductManagement.Models
{
    public class Client
    {
        [Table("Client")]
        public class ClienteModel : User
        {
            [Required]
            public int NIF { get; set; }

            public DateTime BirthDate { get; set; }

            [NotMapped]
            public int Age
            {
                get => (int)Math.Floor((DateTime.Now - BirthDate).TotalDays / 365.2425);
            }

            public ICollection<Address> Addresses { get; set; }

            public ICollection<Request> Requests { get; set; }
        }
    }
}
