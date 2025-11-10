using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MM_api.DTOs;
using MM_api.Models;
using MM_api.Services;
using MM_api.Utils;

namespace MM_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MarathonManagementV1Context _context;
        private readonly JwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IOrganizerService _organizerService;

        public AuthController(
            MarathonManagementV1Context context,
            JwtService jwtService,
            IUserService userService,
            IOrganizerService organizerService)
        {
            _context = context;
            _jwtService = jwtService;
            _userService = userService;
            _organizerService = organizerService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO account)
        {
            if (account == null || string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.PasswordHash))
                return BadRequest("Email và mật khẩu không được để trống.");

            var acc = await _context.Users
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email.ToLower() == account.Email.ToLower());

            if (acc == null || !VerifyPassword(account.PasswordHash, acc.PasswordHash))
                return Unauthorized("Sai thông tin đăng nhập.");

            var accessToken = _jwtService.GenerateToken(acc.Email, acc.Role.RoleName, acc.UserId);
            var refreshToken = _jwtService.GenerateRefreshToken();

            acc.RefreshToken = refreshToken;
            acc.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            _context.Users.Update(acc);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đăng nhập thành công",
                accessToken,
                refreshToken,
                userId = acc.UserId,
                role = acc.Role.RoleName
            });
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token không hợp lệ.");

            var account = await _context.Users
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (account == null)
                return Unauthorized("Refresh token không tồn tại.");

            if (account.RefreshTokenExpiry == null || account.RefreshTokenExpiry <= DateTime.UtcNow)
                return Unauthorized("Refresh token đã hết hạn.");

            var newAccessToken = _jwtService.GenerateToken(account.Email, account.Role.RoleName, account.UserId);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            account.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.User.Email) || string.IsNullOrEmpty(request.User.PasswordHash))
                return BadRequest("Thiếu thông tin người dùng.");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Hash password
                request.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.PasswordHash);

                // Set role
                int roleId = request.IsOrganizer ? 2 : 3;
                request.User.RoleId = roleId;

                // Create user
                var newUser = await _userService.CreateAsync(request.User);

                // If organizer registration
                if (request.IsOrganizer && request.Organizer != null)
                {
                    var organizer = new Organizer
                    {
                        UserId = newUser.UserId,
                        OrganizationName = request.Organizer.OrganizationName
                    };
                    await _organizerService.AddAsync(organizer);
                }

                await transaction.CommitAsync();

                // Auto-login
                var role = await _context.Roles.FindAsync(roleId);
                var accessToken = _jwtService.GenerateToken(newUser.Email, role?.RoleName ?? "User", newUser.UserId);
                var refreshToken = _jwtService.GenerateRefreshToken();

                var userEntity = await _context.Users.FindAsync(newUser.UserId);
                userEntity!.RefreshToken = refreshToken;
                userEntity.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Đăng ký và đăng nhập thành công",
                    accessToken,
                    refreshToken,
                    userId = newUser.UserId,
                    role = role?.RoleName ?? "User"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
