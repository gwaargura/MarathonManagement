using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MM_client.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MM_client.Controllers
{
    public class MarathonsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public MarathonsController(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        // GET: /Marathons
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new MarathonIndexViewModel();

                // --- Get all marathons ---
                var allResponse = await _httpClient.GetAsync($"{_baseUrl}/api/marathons");
                if (allResponse.IsSuccessStatusCode)
                    model.AllMarathons = await allResponse.Content.ReadFromJsonAsync<List<ReadMarathonDTO>>() ?? new();

                // --- Get current organizer marathons ---
                var userId = HttpContext.Session.GetString("UserId");
                var token = HttpContext.Session.GetString("AccessToken");

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var organizerResponse = await _httpClient.GetAsync($"{_baseUrl}/api/organizer/getId/{userId}");
                    if (organizerResponse.IsSuccessStatusCode)
                    {
                        var organizer = await organizerResponse.Content.ReadFromJsonAsync<Organizer>();
                        if (organizer != null)
                        {
                            var myResponse = await _httpClient.GetAsync($"{_baseUrl}/api/marathons/organizer/{organizer.OrganizerId}");
                            if (myResponse.IsSuccessStatusCode)
                                model.MyMarathons = await myResponse.Content.ReadFromJsonAsync<List<ReadMarathonDTO>>() ?? new();
                        }
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi kết nối đến API: {ex.Message}";
                return View(new MarathonIndexViewModel());
            }
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/marathons/detail/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Không thể tải thông tin giải đấu.";
                    return View("Error");
                }

                var marathon = await response.Content.ReadFromJsonAsync<ReadMarathonDetailDTO>();
                if (marathon == null)
                {
                    ViewBag.Error = "Không tìm thấy giải đấu.";
                    return View("Error");
                }

                return View(marathon);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewBag.Error = "Đã xảy ra lỗi trong quá trình tải dữ liệu.";
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Organizer")
            {
                TempData["Error"] = "Chỉ tổ chức mới có thể tạo giải đấu.";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMarathonViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (userId == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng. Hãy đăng nhập lại.";
                    return RedirectToAction("Login", "Auth");
                }

                // 🔹 Get access token from session
                var token = HttpContext.Session.GetString("AccessToken");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                // 🔹 Step 1: Get OrganizerId using your new API
                var organizerResponse = await _httpClient.GetAsync($"{_baseUrl}/api/organizer/getId/{userId}");
                if (!organizerResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Không thể lấy thông tin organizer.";
                    return RedirectToAction("Login", "Auth");
                }

                var organizer = await organizerResponse.Content.ReadFromJsonAsync<Organizer>();

                // 🔹 Step 2: Prepare checkpoints if any
                List<CreateCheckpointDTO>? checkpoints = null;
                if (!string.IsNullOrEmpty(model.CheckpointsJson))
                {
                    checkpoints = JsonSerializer.Deserialize<List<CreateCheckpointDTO>>(model.CheckpointsJson);
                }

                // 🔹 Step 3: Build DTO for marathon creation
                var dto = new CreateMarathonDTO
                {
                    OrganizerId = organizer.OrganizerId, 
                    MarathonName = model.MarathonName,
                    Description = model.Description,
                    Location = model.Location,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    RegistrationFee = model.RegistrationFee,
                    MaxParticipants = model.MaxParticipants,
                    Status = "Upcoming"
                };

                // 🔹 Step 4: Create marathon
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/marathons", dto);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = $"Tạo marathon thất bại: {error}";
                    return View(model);
                }

                var created = await response.Content.ReadFromJsonAsync<ReadMarathonDTO>();

                // 🔹 Step 5: Create checkpoints if available
                if (checkpoints != null && checkpoints.Count > 0)
                {
                    foreach (var (c, index) in checkpoints.Select((c, i) => (c, i)))
                    {
                        c.MarathonId = created.MarathonId;
                        c.Sequence = index + 1;
                    }

                    await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/checkpoints/many", checkpoints);
                }

                TempData["Success"] = "Tạo marathon thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Đã xảy ra lỗi: {ex.Message}";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateMarathonDTO model)
        {
            if (!ModelState.IsValid)
            {
                // If form validation fails, return the same view with current data
                return View(model);
            }

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/marathons/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật thông tin giải đấu thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Không thể cập nhật thông tin giải đấu.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View(model);
            }
        }

        [HttpGet] 
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/marathons/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Không thể tải thông tin giải đấu.";
                    return RedirectToAction("Index");
                }
                var marathon = await response.Content.ReadFromJsonAsync<ReadMarathonDTO>();
                if (marathon == null)
                {
                    TempData["Error"] = "Không tìm thấy giải đấu.";
                    return RedirectToAction("Index");
                }
                var model = new UpdateMarathonDTO
                {
                    MarathonName = marathon.MarathonName,
                    Description = marathon.Description,
                    Location = marathon.Location,
                    StartDate = marathon.StartDate,
                    EndDate = marathon.EndDate,
                    RegistrationFee = marathon.RegistrationFee,
                    MaxParticipants = marathon.MaxParticipants
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(int marathonId)
        {
            try
            {
                var token = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Vui lòng đăng nhập trước khi đăng ký.";
                    return RedirectToAction(nameof(Details), new { id = marathonId });
                }

                // Decode JWT to get user ID
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "AccountId");

                if (userIdClaim == null)
                {
                    TempData["Error"] = "Không thể xác định người dùng từ token.";
                    return RedirectToAction(nameof(Details), new { id = marathonId });
                }

                int userId = int.Parse(userIdClaim.Value);

                // Attach JWT to request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var registration = new
                {
                    UserId = userId,
                    MarathonId = marathonId,
                    Status = "Pending"
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/registrations", registration);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đăng ký thành công!";
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Không thể đăng ký: {response.StatusCode} - {errorMsg}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi đăng ký: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = marathonId });
        }

    }
}
