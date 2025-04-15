using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project_Management_System.Data;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.UserDTO;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;
using Project_Management_System.Options;

namespace Project_Management_System.Services
{
    public class AuthService(AppDbContext dbContext, IOptions<JwtOptions> jwtOptions, IEmail _emailService) : IAuth
    {

        public async Task<object> ChangePassword(string username, string currentPassword, string newPassword)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync((e) => e.Username == username);

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, currentPassword) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Password is Not Correct");
            }
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, newPassword);
            await dbContext.SaveChangesAsync();

            return new { Message = "Your Password Changed Correctly." };
        }
        public async Task<TokenResponseDto> Login(UserLoginDto userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.Username) || string.IsNullOrEmpty(userInfo.Password))
            {
                throw new Exception("nullable values");
            }


            var user = await dbContext.Users.FirstOrDefaultAsync((e) => e.Username.ToLower() == userInfo.Username.ToLower());


            if (user == null)
            {
                throw new Exception("Invalid Password or Username");
            }


            var PasswordHasher = new PasswordHasher<Models.User>();

            if (PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userInfo.Password)
                == PasswordVerificationResult.Failed)
            {

                throw new Exception("Invalid Password or Username");
            }

            // Will Be saved, cuz user is passed by ref.
            user.LastLogin = DateTime.Now;

            return await CreateResponseTokenAsync(user);
        }
        public System.Threading.Tasks.Task Logout()
        {
            throw new NotImplementedException();
        }
        public async Task<TokenResponseDto> RefreshToken(RefreshTokenRequestDto request)
        {
            // Validate the refresh token
            var user = await ValidateRefreshToken(request);

            // If the refresh token is valid, generate new tokens
            if (user != null)
            {
                // Return the token response DTO with valid tokens
                return await CreateResponseTokenAsync(user);
            }

            // If refresh token is invalid, throw an exception
            throw new Exception("Unable to refresh token.");
        }
        public async Task<object> Register(UserRegistrationDto userInfo)
        {

            if (string.IsNullOrEmpty(userInfo.Username) || string.IsNullOrEmpty(userInfo.Password) || string.IsNullOrEmpty(userInfo.Email))
            {
                throw new Exception("Invalid Credentrials");
            }

            if (await dbContext.Users.AnyAsync((e) => e.Username == userInfo.Username))
            {
                throw new Exception("Your username is already in use.");
            }



            User newUser = new();
            string HashedPassword = new PasswordHasher<User>().HashPassword(newUser, userInfo.Password);



            newUser.Email = userInfo.Email;
            newUser.PasswordHash = HashedPassword;
            newUser.Username = userInfo.Username;
            Send2FACode(newUser);


            await dbContext.Users.AddAsync(newUser);
            var rowsAffected = await dbContext.SaveChangesAsync();

            if (rowsAffected <= 0)
            {
                throw new Exception("failed to add new user");
            }

            return new { Message = $"We Have Sent Verfication Code To Your Email {newUser.Email}, Enter The Code To This Link ................. Then Complete Login." };
            //  return await CreateResponseTokenAsync(newUser);
        }
        public async Task<object> ResetPassword(string username, string email, string newPassword)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync((e) => e.Username == username);


            /*
             * in front-end you should get the Entered2FACode, and pass it to this function
             * but here, I just work on the back-end.
             * so I'll assume the token is correct.
               Send2FACode(user);
             */


            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, newPassword);
            if (user is null)
            {
                throw new Exception("User is not found");
            }
            await dbContext.SaveChangesAsync();
            return new { Message = "Your Password Changed Correctly." };
        }
        public bool Send2FACode(User user)
        {
            user.TwoFactorAuthentication = _emailService.Generate2FAToken();
            user.TwoFactorAuthenticationExpiration = DateTime.Now.AddMinutes(3);

            //if (!_emailService.SendEmail(user.Email, "2 Factor Authentication Code", $"Your Code is: \n\n\t{user.TwoFactorAuthentication}"))
            //{
            //    return false;
            //}

            bool isEmailSent = _emailService.SendEmail(user.Email, "Two Factor Authentication Code", user.TwoFactorAuthentication);
            if (isEmailSent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<TokenResponseDto> Validate2FACode(string username, string Token)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync((e) => e.Username == username);
            if (user == null || user.TwoFactorAuthentication is null || user.TwoFactorAuthenticationExpiration <= DateTime.UtcNow)
            {
                throw new Exception("Error While Verify the User");
            }

            if (user.TwoFactorAuthentication != Token)
            {
                throw new Exception("Not Matched Codes");


            }
            user.TwoFactorAuthentication = null;
            user.TwoFactorAuthenticationExpiration = null;
            return await CreateResponseTokenAsync(user);
        }
        // -----------------------------------
        private async Task<User> ValidateRefreshToken(RefreshTokenRequestDto request)
        {
            var user = await dbContext.Users.FindAsync(request.Id);

            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Invalid Refresh Token, Re-login Please");
            }
            return user;
        }
        private async Task<TokenResponseDto> CreateResponseTokenAsync(User user)
        {

            return new TokenResponseDto()
            {
                AccessToken = await GenerateAccessToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)

            };


        }
        private async Task<string> GenerateAccessToken(Models.User user)
        {

            var Claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SigningKey));


            var SigningCreds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: Claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: SigningCreds
                        );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return jwtToken;

        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(Models.User user)
        {
            string RefreshToken = CreateRandomRefreshToken().Result;
            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await dbContext.SaveChangesAsync();
            return RefreshToken;


        }
        private Task<string> CreateRandomRefreshToken()
        {
            // استخدام GUID لتوليد قيمة فريدة وآمنة
            string refreshToken = Guid.NewGuid().ToString();

            // إرجاع الـ Refresh Token
            return System.Threading.Tasks.Task.FromResult(refreshToken);
        }

    }
}
