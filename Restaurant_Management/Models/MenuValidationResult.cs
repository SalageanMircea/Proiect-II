namespace Restaurant_Management.Models
{
    public class MenuValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; } = "";

        public MenuProduct Product { get; set; } = new MenuProduct();

        public static MenuValidationResult Success(MenuProduct product)
        {
            return new MenuValidationResult
            {
                IsValid = true,
                ErrorMessage = "",
                Product = product
            };
        }

        public static MenuValidationResult Failed(string message)
        {
            return new MenuValidationResult
            {
                IsValid = false,
                ErrorMessage = message,
                Product = new MenuProduct()
            };
        }
    }
}