using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private IRateService _rateService;

        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpGet("Get-All-Rate")]
        public async Task<IActionResult> GetAllRate()
        {
            try
            {
                return Ok(await _rateService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không tìm được dữ liệu");
            }

        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<Rate>> Get(Guid id)
        {
            try
            {
                return Ok(await _rateService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-rate")]
        public async Task<ActionResult<Rate>> CreateRate([FromBody] Rate rate)
        {
            var result = await _rateService.Create(rate);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-rate-{id}")]
        public async Task<ActionResult<Rate>> Put(Guid orderid, int star, string? comment)
        {
            var result = await _rateService.Update(orderid, star, comment);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-rate-{id}")]
        public async Task<ActionResult<Rate>> Delete(Guid id, Guid orderid)
        {
            var result = await _rateService.Delete(id, orderid);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
