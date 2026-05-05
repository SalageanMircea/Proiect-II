namespace Restaurant_Management.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Message { get; set; } = "";

        public AppUser? User { get; set; }

        public static LoginResult Failed(string message)
        {
            return new LoginResult
            {
                Success = false,
                Message = message,
                User = null
            };
        }

        public static LoginResult Ok(AppUser user)
        {
            return new LoginResult
            {
                Success = true,
                Message = "",
                User = user
            };
        }
    }
}