using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Display(Name = "List price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 500)]
        [Display(Name = "Price for 1-50+")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 51-100+")]
        [Range(1, 500)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 500)]
        public double Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ValidateNever]
        [Display(Name = "Category")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public Guid CoverTypeId { get; set; }

        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
