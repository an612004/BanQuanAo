using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/size")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private ISizeService _iSizeService;

        public SizeController(ISizeService iSizeService)
        {
            _iSizeService = iSizeService;
        }
        [HttpGet("get-all-size")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iSizeService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<Size>> Get(Guid id)
        {
            try
            {
                return Ok(await _iSizeService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-size")]
        public async Task<ActionResult<SizeVM>> Post([FromBody] SizeVM obj)
        {
            var result = await _iSizeService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPost("create-many-size")]
        public async Task<ActionResult<SizeVM>> PostMany([FromBody] List<SizeVM> obj)
        {
            var result = await _iSizeService.CreateMany(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-size-{id}")]
        public async Task<ActionResult<SizeVM>> Put(Guid id, [FromBody] SizeVM obj)
        {
            var result = await _iSizeService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-size-{id}")]
        public async Task<ActionResult<SizeVM>> Delete(Guid id)
        {
            var result = await _iSizeService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
