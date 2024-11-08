using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
            public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _roleService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string roleName)
        {
            try
            {
                return Ok(await _roleService.GetItem(roleName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-role")]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            var result = await _roleService.Create(role);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleName)
        {
            var result = await _roleService.Delete(roleName);
            if (result)
            {
                return Ok("Đã xóa thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
