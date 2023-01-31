using SimpleWebStore.Domain.Abstractions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebStore.Domain.Entities
{
    public class Category : DbEntity
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}