using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BanQuanAo.API.Controllers
{
    [Route("api/cartitem")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private ICartItemService _cartItemService;
        public CartItemController(ICartItemService iCartItemService)
        {
            _cartItemService = iCartItemService;
        }
        // GET: api/<CartItemController>
        [HttpGet("Get-All-CartItem")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _cartItemService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không tìm được dữ liệu");
            }
        }

        // GET api/<CartItemController>/5
        [HttpGet("Get-CartItem-ById")]
        public async Task<ActionResult<CartItem>> GetById(Guid id)
        {
            try
            {
                return Ok(await _cartItemService.GetCartItemsByCartIds(id));
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        // GET api/<CartItemController>/5
        [HttpGet("getcartitembycartid")]
        public async Task<ActionResult<CartItem>> GetCartItemByCartId(Guid cartid)
        {
            try
            {
                return Ok(await _cartItemService.GetAllCartItemsByCartId(cartid));
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        // POST api/<CartItemController>
        [HttpPost("Insert-Cart-Item")]
        public async Task<ActionResult<CartItem>> Post([FromBody] CartItem obj)
        {
            var result = await _cartItemService.AddCartItem(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return BadRequest("Lỗi");
        }

        // PUT api/<CartItemController>/5
        [HttpPut("Update-CartItem")]
        public async Task<ActionResult<CartItem>> Put(Guid id, [FromBody] CartItem obj)
        {
            var result = await _cartItemService.UpdateCartItem(id, obj.Quantity, obj.Price);
            if (result)
            {
                return Ok("Sửa thành công");
            }
            return BadRequest("Lỗi");
        }
        [HttpGet("UpdateQuantityCartItem")]
        public async Task<ActionResult<CartItem>> UpdateQuantityCartItem(Guid cartItemId,int quantity)
        {
            var result = await _cartItemService.UpdateQuantity(cartItemId, quantity);
            if (result)
            {
                return Ok("Sửa thành công");
            }
            return BadRequest("Lỗi");
        }

        // DELETE api/<CartItemController>/5
        [HttpDelete("Delete-CartItem")]
        public async Task<ActionResult<CartItem>> Delete(Guid cartId)
        {
            var result = await _cartItemService.DeleteCartItem(cartId);
            if (result)
            {
                return Ok("Xóa thành công");
            }
            return BadRequest("Lỗi");
        }
    }
}
