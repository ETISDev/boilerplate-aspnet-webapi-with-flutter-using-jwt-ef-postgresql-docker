namespace CoreApi.DTOs.Auth
{
    public class UserLoginDTO
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

}