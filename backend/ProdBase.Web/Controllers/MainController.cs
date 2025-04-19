using Microsoft.AspNetCore.Mvc;

namespace ProdBase.Web.Controllers
{
    [ApiController]
    [Route("/")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                message = "ユーザープロフィールAPIが実行中です",
                endpoints = new
                {
                    auth_verify = "/api/auth/verify (Authorizationヘッダーを持つPOST)",
                    profile_get = "/api/profile/ (Authorizationヘッダーを持つGET)",
                    profile_update = "/api/profile/ (JSONボディとAuthorizationヘッダーを持つPUT)"
                }
            });
        }
    }
}
