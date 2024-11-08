using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get all User
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _userService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpGet("get/{userName}")]
        public async Task<IActionResult> GetByName(string userName)
        {
            try
            {
                return Ok(await _userService.GetItem(userName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpGet("getID/{userName}")]
        public async Task<IActionResult> GetById(Guid userName)
        {
            try
            {
                return Ok(await _userService.GetItemid(userName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpDelete("delete/{userName}")]
        public async Task<IActionResult> Delete(string userName)
        {
            try
            {
                return Ok(await _userService.Delete(userName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel item,string roleName) 
        {
            try
            {
                return Ok(await _userService.Create(item, roleName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserViewModel item)
        {
            try
            {
                return Ok(await _userService.Update(item));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("change-role/{userName}/{roleName}")]
        public async Task<IActionResult> ChangeRole(string userName, string roleName)
        {
            try
            {
                return Ok(await _userService.ChangeRole(userName, roleName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("reset-password/{userName}/{newPassword}")]
        public async Task<IActionResult> ResetPassword(string userName, string newPassword)
        {
            try
            {
                return Ok(await _userService.ResetPassword(userName, newPassword));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("change-password/{userName}/{currentPassword}/{newPassword}")]
        public async Task<IActionResult> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            try
            {
                return Ok(await _userService.ChangePassword(userName, currentPassword, newPassword));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("check-password/{userName}/{currentPassword}")]
        public async Task<IActionResult> CheckPassword(string userName, string currentPassword)
        {
            try
            {
                return Ok(await _userService.CheckPassword(userName, currentPassword));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
    }
}
