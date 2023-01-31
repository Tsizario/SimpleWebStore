namespace SimpleWebStore.Domain.Constants
{
    public class Errors
    {
        public const string CategoryNotFound = "Category not found";
        public const string CategoryAddingError = "Error while creating category";
        public const string CategorySameNumber = "The order's number cannot exactly match the same name";
        public const string CategoryDoesNotExist = "Category doesn't exist";
        public const string CategoriesDoesNotExist = "Categories doesn't exist";

        public const string CoverTypeNotFound = "Cover type not found";
        public const string CoverTypeAddingError = "Error while adding cover type";
        public const string CoverTypeDoesNotExist = "Cover type doesn't exist";

        public const string ProductNotFound = "Product not found";
        public const string ProductAddingError = "Error while adding product";
        public const string ProductDoesNotExist = "Product doesnot exist";

        public const string PhotoAddingError = "Error while adding photo";
        public const string PhotoDoesNotExists = "The photo does not exists";
    }
}
