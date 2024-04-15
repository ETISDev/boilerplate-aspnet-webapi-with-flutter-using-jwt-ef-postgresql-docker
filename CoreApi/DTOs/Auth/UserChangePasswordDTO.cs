namespace CoreApi.DTOs.Auth
{
    public class UserChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }

}