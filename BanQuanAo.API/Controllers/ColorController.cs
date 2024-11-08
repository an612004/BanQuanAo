using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BanQuanAo.API.Controllers
{
    [Route("api/color")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private IColorService _iColorService;

        public ColorController(IColorService iColorService)
        {
            _iColorService = iColorService;
        }
        [HttpGet("get-all-Color")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iColorService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<Colors>> Get(Guid id)
        {
            try
            {
                return Ok(await _iColorService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-Color")]
        public async Task<ActionResult<Colors>> Post([FromBody] Colors obj)
        {
            var result = await _iColorService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }

        [HttpPut("update-Color-{id}")]
        public async Task<ActionResult<Colors>> Put(Guid id, [FromBody] Colors obj)
        {
            var result = await _iColorService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-Color-{id}")]
        public async Task<ActionResult<Colors>> Delete(Guid id)
        {
            var result = await _iColorService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
