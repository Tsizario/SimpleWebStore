using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SimpleWebStore.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebStore.Domain.Entities
{
    public class ShoppingCart : DbEntity
    {
        public Guid ProductId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser AppUser { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
    }
}
