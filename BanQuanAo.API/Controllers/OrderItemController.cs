using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/orderitem")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        public IOrderItemService _iOrderItemService;
        public OrderItemController(IOrderItemService iOrderItemService)
        {
            _iOrderItemService = iOrderItemService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iOrderItemService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<OrderItem>> Get(Guid id)
        {
            try
            {
                return Ok(await _iOrderItemService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<OrderItemVM>> Post([FromBody] OrderItemVM obj)
        {
            var result = await _iOrderItemService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        
        [HttpPut("update-product-{id}")]
        public async Task<ActionResult<OrderItemVM>> Put(Guid id, [FromBody] OrderItemVM obj)
        {
            var result = await _iOrderItemService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-{id}")]
        public async Task<ActionResult<OrderItemVM>> Delete(Guid id)
        {
            var result = await _iOrderItemService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
