

namespace CoreApi.Interfaces
{
    public interface IAuthHelperRepository
    {
        string GenerateJwtToken(UserTokenReadDto user, IList<string> roles);
        string PasswordGenerator();
    }

    class AuthHelperRepository : IAuthHelperRepository
    {
        private readonly IConfiguration _configuration;

        public AuthHelperRepository(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserTokenReadDto user, IList<string> roles)
        {
            // var jsonString = JsonConvert.SerializeObject(user);
            var claims = new List<Claim>
    {
        // new("user", jsonString),
        new("id", user.Id.ToString()),
        new("userName", user.UserName),
        new("firstName", user.FirstName),
        new("lastName", user.LastName),
        new("profileImageUrl", user.ProfileImageUrl??""),
    };

            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value ?? ""));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            // .Result;
        }


        public string PasswordGenerator()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (long i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new string(stringChars);
            return finalString;
        }
    }
}