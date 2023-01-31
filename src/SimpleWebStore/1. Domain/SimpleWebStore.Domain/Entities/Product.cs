using SimpleWebStore.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebStore.Domain.Entities
{
    public class Product : DbEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 500)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 500)]
        public double Price { get; set; }

        [Required]
        [Range(1, 500)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 500)]
        public double Price100 { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public Guid CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }
    }
}
