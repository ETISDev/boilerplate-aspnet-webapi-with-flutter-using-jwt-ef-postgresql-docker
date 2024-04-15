namespace CoreApi.DTOs.Auth
{
    public class UserChangeUserNameDTO
    {
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
    }

}