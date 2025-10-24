using Marathon.Models;
using Marathon.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Marathon.Dtos.AccountDto;

namespace Marathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MarathonManagementContext _context;
        private readonly JwtService _jwtService;
        public AuthController(MarathonManagementContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountDto account)
        {
            if (account == null || string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.PasswordHash))
                return BadRequest("Email và mật khẩu không được để trống.");

            // Tìm account
            var acc = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email == account.Email && a.PasswordHash == account.PasswordHash);

            if (acc == null)
                return Unauthorized("Invalid credentials");

            // Tạo access token và refresh token
            var accessToken = _jwtService.GenerateToken(acc.Email, acc.Role.RoleName);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Lưu refresh token vào DB
            acc.RefreshToken = refreshToken;
            acc.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // token sống 7 ngày

            _context.Accounts.Update(acc);
            await _context.SaveChangesAsync();

            // Trả về cho client
            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token không hợp lệ.");

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (account == null)
                return Unauthorized("Refresh token không tồn tại.");

            if (account.RefreshTokenExpiryTime == null || account.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token đã hết hạn.");

            // ✅ Sinh token mới
            var newAccessToken = _jwtService.GenerateToken(account.Email, account.Role.RoleName);
            var newRefreshToken = _jwtService.GenerateRefreshToken(); // cần thêm hàm này bên JwtService

            // ✅ Cập nhật lại refresh token trong DB
            account.RefreshToken = newRefreshToken;
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // ví dụ: 7 ngày

            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


    }
}
