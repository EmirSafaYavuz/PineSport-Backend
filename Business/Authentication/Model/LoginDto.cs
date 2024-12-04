namespace Business.Authentication.Model
{
    /// <summary>
    /// DTO for login with email and password.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }
    }
}