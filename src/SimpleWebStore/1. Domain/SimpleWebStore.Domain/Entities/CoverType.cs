using SimpleWebStore.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebStore.Domain.Entities
{
    public class CoverType : DbEntity
    {
        [Display(Name = "Cover Type")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
