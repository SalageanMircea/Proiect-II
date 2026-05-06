namespace Restaurant_Management.Models
{
    public class ShoppingValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; } = "";

        public ShoppingItem Item { get; set; } = new ShoppingItem();

        public static ShoppingValidationResult Success(ShoppingItem item)
        {
            return new ShoppingValidationResult
            {
                IsValid = true,
                ErrorMessage = "",
                Item = item
            };
        }

        public static ShoppingValidationResult Failed(string message)
        {
            return new ShoppingValidationResult
            {
                IsValid = false,
                ErrorMessage = message,
                Item = new ShoppingItem()
            };
        }
    }
}