using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleWebStore.Domain.Entities
{
    public class OrderDetail
    {
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}
