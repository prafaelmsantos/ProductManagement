using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProductManagement.Models
{
    [Table("Request")]
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public double? TotalPrice { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public Address DeliveryAddress { get; set; }

        public ICollection<ItemOrder> ItemOrders { get; set; }
    }
}
