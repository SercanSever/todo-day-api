namespace TO_DO.ENTİTY.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public string PasswordEncode { get; set; }
        public bool IsActive { get; set; }
    }
}