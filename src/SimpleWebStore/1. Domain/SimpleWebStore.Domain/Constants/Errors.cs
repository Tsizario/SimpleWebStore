namespace SimpleWebStore.Domain.Constants
{
    public class Errors
    {
        public const string CategoryNotFound = "Category not found";
        public const string CategoryAddingError = "Error while creating category";
        public const string CategorySameNumber = "Order's number cannot exactly match the same name";
        public const string CategoryDoesNotExist = "Category doesn't exist";
        public const string CategoriesDoesNotExist = "Categories doesn't exist";

        public const string CoverTypeNotFound = "Cover type not found";
        public const string CoverTypeAddingError = "Error while adding cover type";
        public const string CoverTypeDoesNotExist = "Cover type doesn't exist";

        public const string ProductNotFound = "Product not found";
        public const string ProductAddingError = "Error while adding product";
        public const string ProductDeletingError = "Error while deleting product";
        public const string ProductDoesNotExist = "Product does not exist";

        public const string PhotoAddingError = "Error while adding photo";
        public const string PhotoDoesNotExists = "Photo does not exists";

        public const string CompanyNotFound = "Company not found";
        public const string CompanyAddingError = "Error while adding company";
        public const string CompanyDeletingError = "Error while deleting company";
        public const string CompanyDoesNotExist = "Company does not exist";

        public const string ShoppingCartEmpty = "Shopping cart is empty!";
    }
}
