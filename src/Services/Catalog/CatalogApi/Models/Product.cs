using System.ComponentModel.DataAnnotations;

namespace CatalogApi.Models
{
    public class Product
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
        public List<string> Category { get; set; } = new();
    }
}
