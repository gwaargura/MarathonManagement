using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MM_client.Models;
using System.Text;
using System.Text.Json;

namespace MM_client.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthController(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerRequest = new
            {
                User = new
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Email = model.Email,
                    FullName = model.FullName,
                    RoleId = 0
                },
                IsOrganizer = model.IsOrganizer,
                Organizer = model.IsOrganizer
                    ? new { UserId = 0, OrganizationName = model.OrganizationName }
                    : null
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/auth/register", registerRequest);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<dynamic>();
                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Đăng ký thất bại: {error}");
            return View(model);
        }



        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginRequest = new
            {
                Email = model.Email,
                PasswordHash = model.Password
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
                TempData["Success"] = "Đăng nhập thành công!";

                // Save JWT and user info into session
                HttpContext.Session.SetString("AccessToken", data?.AccessToken ?? "");
                HttpContext.Session.SetString("RefreshToken", data?.RefreshToken ?? "");
                HttpContext.Session.SetString("UserId", data?.UserId.ToString() ?? "");
                HttpContext.Session.SetString("Role", data?.Role ?? "");

                if (string.Equals(data?.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Index", "Marathons");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Đăng nhập thất bại: {error}");
            return View(model);
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đã đăng xuất.";
            return RedirectToAction("Login");
        }

    }
}
