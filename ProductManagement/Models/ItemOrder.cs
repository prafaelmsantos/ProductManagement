using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models
{
    [Table("ItemOrder")]
    public class ItemOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RequestId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public double UnitaryValue { get; set; }

        [ForeignKey("RequestId")]
        public Request Request { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [NotMapped]
        public double ItemValue { get => this.Quantity * this.UnitaryValue; }
    }
}
