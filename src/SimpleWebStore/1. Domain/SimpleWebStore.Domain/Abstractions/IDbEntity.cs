using System.ComponentModel.DataAnnotations;

namespace SimpleWebStore.Domain.Abstractions
{
    public interface IDbEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}