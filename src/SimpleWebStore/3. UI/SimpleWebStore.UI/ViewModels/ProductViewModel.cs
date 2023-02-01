using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.UI.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
