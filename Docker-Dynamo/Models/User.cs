namespace Docker_Dynamo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
    }
    public class UserDTO 
    {
        public string EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
    }


    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
