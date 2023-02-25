using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.UI.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<ShoppingCart> ListCart { get; set; }
    }
}
