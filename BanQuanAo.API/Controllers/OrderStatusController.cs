using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/orderstatus")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        public IOrderStatusService _iorderStatusService;
        public OrderStatusController(IOrderStatusService  orderStatusService) {
            _iorderStatusService = orderStatusService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iorderStatusService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<OrderStatus>> Get(Guid id)
        {
            try
            {
                return Ok(await _iorderStatusService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<OrderStatusVM>> Post([FromBody] OrderStatusVM obj)
        {
            var result = await _iorderStatusService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
       
        [HttpPut("update-{id}")]
        public async Task<ActionResult<OrderStatusVM>> Put(Guid id, [FromBody] OrderStatusVM obj)
        {
            var result = await _iorderStatusService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-{id}")]
        public async Task<ActionResult<OrderStatusVM>> Delete(Guid id)
        {
            var result = await _iorderStatusService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
