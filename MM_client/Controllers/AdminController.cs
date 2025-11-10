using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MM_client.Models;
using System.Text;
using System.Text.Json;

namespace MM_client.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AdminController(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        // === Dashboard default ===
        public async Task<IActionResult> Dashboard()
        {
            // Load Users by default
            var users = await FetchUsers();
            return View(users);
        }

        private async Task<List<ReadUserDTO>> FetchUsers()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Users");
            if (!response.IsSuccessStatusCode)
                return new List<ReadUserDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ReadUserDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        // === AJAX endpoint to load Users section ===
        public async Task<IActionResult> UsersPartial()
        {
            var users = await FetchUsers();
            return PartialView("_UsersPartial", users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Users/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Dashboard");

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<ReadUserDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var updateDto = new UpdateUserDTO
            {
                Email = user.Email,
                FullName = user.FullName,
                IsActive = user.IsActive
            };

            ViewBag.UserId = user.UserId;
            ViewBag.Username = user.Username;
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, UpdateUserDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Users/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Dashboard");

            ViewBag.Error = "Update failed.";
            return View(dto);
        }

        // ========================= MARATHONS =========================
        private async Task<List<ReadMarathonDTO>> FetchMarathons()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Marathons");
            if (!response.IsSuccessStatusCode)
                return new List<ReadMarathonDTO>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ReadMarathonDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        public async Task<IActionResult> MarathonsPartial()
        {
            var marathons = await FetchMarathons();
            return PartialView("_MarathonsPartial", marathons);
        }


        [HttpGet]
        public async Task<IActionResult> EditMarathon(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Marathons/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Dashboard");

            var json = await response.Content.ReadAsStringAsync();
            var marathon = JsonSerializer.Deserialize<ReadMarathonDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var updateDto = new UpdateMarathonDTO
            {
                MarathonName = marathon.MarathonName,
                Location = marathon.Location,
                StartDate = marathon.StartDate,
                EndDate = marathon.EndDate,
                Status = marathon.Status
            };

            ViewBag.MarathonId = marathon.MarathonId;
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditMarathon(int id, UpdateMarathonDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Marathons/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Dashboard");

            ViewBag.Error = "Update failed.";
            return View(dto);
        }
    }
}
