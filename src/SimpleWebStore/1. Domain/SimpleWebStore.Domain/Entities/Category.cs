using SimpleWebStore.Domain.Abstractions;

namespace SimpleWebStore.Domain.Entities
{
    public class Category : DbEntity
    {
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}