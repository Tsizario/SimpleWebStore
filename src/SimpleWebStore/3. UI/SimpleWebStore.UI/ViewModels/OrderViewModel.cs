using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.UI.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
