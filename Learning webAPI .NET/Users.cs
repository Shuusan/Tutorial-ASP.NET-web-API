namespace Learning_webAPI_.NET
{
    public class Users
    {

        public Users(DateTime dateRegister, string? email, string? username)
        {
            DateRegister = dateRegister;
            Email = email;
            Username = username;
        }
        public DateTime DateRegister { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
    }
}