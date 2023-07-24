using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}
