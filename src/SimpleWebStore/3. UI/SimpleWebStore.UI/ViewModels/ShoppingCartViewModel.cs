using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.UI.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }

        public double CartTotal { get; set; }
    }
}
