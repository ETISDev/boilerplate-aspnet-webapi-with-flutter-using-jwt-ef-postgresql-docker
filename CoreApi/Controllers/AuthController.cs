
namespace CoreApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthHelperRepository _authHelper;
        private readonly IMapper _mapper;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly UserManager<UserInfo> _userManager;

        public AuthController(
            IAuthHelperRepository authHelper,
            IMapper mapper,
            UserManager<UserInfo> userManager,
            SignInManager<UserInfo> signInManager
        )
        {
            _authHelper = authHelper;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userForLoginDto)
        {
            try
            {
                var message = new
                {
                    message = "Username or Password is incorrect"
                };
                var user = await _userManager.FindByNameAsync(userForLoginDto.Username);
                if (user == null) return Unauthorized(message);
                if (user.IsDeleted) return Unauthorized(message);

                var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, true);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return Ok(new
                    {
                        token = _authHelper.GenerateJwtToken(_mapper.Map<UserTokenReadDto>(user), roles),
                        message = "Login successfully",
                    });
                }

                Console.WriteLine(result.IsLockedOut);
                return Unauthorized(message);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [Authorize(Roles = "Admin, SuperUser")]
        [HttpPut("ResetPassword/{userId}")]
        public async Task<ActionResult> ResetPassword(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Unauthorized(new
            {
                message = "User Information Not Found"
            });

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetPassword = _authHelper.PasswordGenerator();
            var resultOfResetPassword = await _userManager.ResetPasswordAsync(user, resetToken, resetPassword);
            if (resultOfResetPassword.Succeeded)
            {
                return Ok(new
                {
                    resetpassword = resetPassword,
                    message = "Password reseated successfully",
                });
            }
            return Unauthorized(resultOfResetPassword.Errors);
        }


        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(UserChangePasswordDTO userChangePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(User?.FindFirst("id")?.Value ?? "0");
            if (user == null) return Unauthorized(new { message = "User Not Found" });
            Console.WriteLine(userChangePasswordDto.OldPassword);
            Console.WriteLine(userChangePasswordDto.NewPassword);
            Console.WriteLine(user.Id);
            var result = await _signInManager.CheckPasswordSignInAsync(user, userChangePasswordDto.OldPassword, false);
            Console.WriteLine(result.Succeeded);
            if (result.Succeeded)
            {
                var resultOfChangePassword = await _userManager.ChangePasswordAsync(user, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);
            Console.WriteLine(resultOfChangePassword.Errors.FirstOrDefault()?.Description);
            Console.WriteLine(resultOfChangePassword.Succeeded);
                if (resultOfChangePassword.Succeeded)
                {
                    return Ok(new { message = "Password changed successfully" });
                }
                return Unauthorized(resultOfChangePassword.Errors);
            }
            return Unauthorized(new { message = "Old Password is incorrect" });
        }

        [HttpPut("ChangeUsername")]
        public async Task<ActionResult> ChangeUsername(UserChangeUserNameDTO userChangeUserNameDto)
        {
            long Id = long.Parse(User?.FindFirst("id")?.Value ?? "0");
            var isUser = await _userManager.FindByNameAsync(userChangeUserNameDto.UserName);
            if (isUser != null) return Unauthorized("Username not available");
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return Unauthorized("User Not Found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, userChangeUserNameDto.Password, false);
            if (result.Succeeded)
            {
                var resultOfUserName = await _userManager.SetUserNameAsync(user, userChangeUserNameDto.UserName);
                if (resultOfUserName.Succeeded)
                {
                    return Ok(new { message = "Username changed successfully" });
                }
                return Unauthorized(resultOfUserName.Errors);
            }
            return Unauthorized("Password is incorrect");
        }
    }
}