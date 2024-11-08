using BanQuanAo.API.IServices;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _userAuthentication;


        public AuthenticationController(IAuthenticationService userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }
        [HttpPost("login")]
        
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _userAuthentication.Login(model);
            if (result.IsSuccess)
            {
                return Ok(result.Token);
            }
            else
            {
                return BadRequest(result.Messages);
            }
        }
        [HttpPost("register")]
        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _userAuthentication.Register(model);
            return Ok(result);
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userAuthentication.Logout();
            return Ok(new { Message = "Đăng xuất thành công." });
        }
    }
}
