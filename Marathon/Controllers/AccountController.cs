using Marathon.Models;
using Marathon.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MarathonManagementContext _context;
        private readonly JwtService _jwtService;
    }
}
