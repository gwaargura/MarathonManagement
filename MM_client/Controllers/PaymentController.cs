using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MM_client.Models;
using MM_client.Models.MoMo;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MM_client.Controllers
{
    public class PaymentController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private CollectionLinkRequest _request = new CollectionLinkRequest();
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public PaymentController(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(int marathonId)
        {

            string accessKey = "F8BBA842ECF85";
            string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

            _request.orderId = marathonId + "";
            _request.orderInfo = $"Order [{_request.orderId}]";

            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Marathons/{marathonId}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Marathon");

            var json = await response.Content.ReadAsStringAsync();

            var marathon = JsonSerializer.Deserialize<ReadMarathonDTO>(json,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _request.amount = (long)(marathon.RegistrationFee);
            _request.Items = new List<MoMoItem>
            {
                new MoMoItem
                {
                    Name = marathon.MarathonName,
                    Price = (long)marathon.RegistrationFee,
                    Quantity = 1
                }
            };

            _request.lang = "vi";

            var response1 = await _httpClient.GetAsync($"{_baseUrl}/api/Users/{HttpContext.Session.GetString("UserId")}");
            if (!response1.IsSuccessStatusCode)
                return RedirectToAction("Marathon");

            var json1 = await response.Content.ReadAsStringAsync();

            var userInfo = JsonSerializer.Deserialize<ReadUserDTO>(json1,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });



            _request.UserInfo = new MoMoUserInfo { Name = userInfo.FullName, PhoneNumber = "0979268444", Address = userInfo.Email };
            _request.partnerCode = "MOMO";
            _request.redirectUrl = "http://localhost:7269/Payment/Result";
            _request.ipnUrl = "localhost:7269/Payment/Result";
            _request.requestId = _request.orderId;
            _request.requestType = "payWithMethod";
            _request.extraData = "";
            _request.storeId = "Marathon Management";
            _request.autoCapture = true;
            //_request.amount = 1000;

            var rawSignature = "accessKey=" + accessKey
                + "&amount=" + _request.amount
                + "&extraData=" + _request.extraData
                + "&ipnUrl=" + _request.ipnUrl
                + "&orderId=" + _request.orderId
                + "&orderInfo=" + _request.orderInfo
                + "&partnerCode=" + _request.partnerCode
                + "&redirectUrl=" + _request.redirectUrl
                + "&requestId=" + _request.requestId
                + "&requestType=" + _request.requestType;
            _request.signature = getSignature(rawSignature, secretKey);

            StringContent httpContent = new StringContent(JsonSerializer.Serialize(_request), System.Text.Encoding.UTF8, "application/json");
            var quickPayResponse = await client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);
            var contentsString = await quickPayResponse.Content.ReadAsStringAsync();

            //Console.WriteLine("-----------------------------------------------------------------");
            //Console.WriteLine(contentsString);
            //Console.WriteLine("-----------------------------------------------------------------");

            var response2 = JsonSerializer.Deserialize<PaymentResult>(contentsString);

            if (response2 == null)
            {
                TempData["error"] = "Không thể tạo link thanh toán";
                return View("Index");
            }

            if (response2.ResultCode == 0)
            {
                var token = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Vui lòng đăng nhập trước khi đăng ký.";
                    return RedirectToAction("Login", "Auth");
                }

                // Decode JWT to get user ID
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "AccountId");

                if (userIdClaim == null)
                {
                    TempData["Error"] = "Không thể xác định người dùng từ token.";
                    return RedirectToAction("Login", "Auth");
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

                var response3 = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/registrations", registration);

                if (response3.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đăng ký thành công!";
                }
                else
                {
                    var errorMsg = await response3.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Không thể đăng ký: {response3.StatusCode} - {errorMsg}";
                }
            }
            TempData["message"] = "Giao dịch của bạn đang được xử lý, nếu bạn không tự động được chuyển đến trang thanh toán, vui lòng nhấn vào đường dẫn.";
            TempData["payUrl"] = response2.PayUrl;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Result(string orderId, string requestId, string resultCode, string message, string partnerCode, long amount, long responseTime, string payUrl, string shortLink)
        {

            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Vui lòng đăng nhập trước khi đăng ký.";
                return RedirectToAction("Login", "Auth");
            }

            // Decode JWT to get user ID
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "AccountId");

            if (userIdClaim == null)
            {
                TempData["Error"] = "Không thể xác định người dùng từ token.";
                return RedirectToAction("Login", "Auth");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Attach JWT to request
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var registration = new
            {
                Status = "Completed"
            };

            var response3 = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/registrations/{orderId}", registration);

            if (response3.IsSuccessStatusCode)
            {
                TempData["Success"] = "Đăng ký thành công!";
            }
            else
            {
                var errorMsg = await response3.Content.ReadAsStringAsync();
                TempData["Error"] = $"Không thể đăng ký: {response3.StatusCode} - {errorMsg}";
            }

            if (resultCode.Equals("0"))
            {
                TempData["message"] = "Đơn đăng ký của bạn đã được xác nhận thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Đơn đăng ký của bạn đã bị hủy do quá thời gian thanh toán.";
                return RedirectToAction("Index");
            }
        }

        private static string getSignature(string text, string key)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);

            using HMACSHA256 hash = new HMACSHA256(keyBytes);
            byte[] hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
