using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private IWishListService _wishListService;
        private IProductService _productService;
        public WishListController(IWishListService wishListService, IProductService productService)
        {
            _wishListService = wishListService;
            _productService = productService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _wishListService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<WishList>> Get(Guid id)
        {
            try
            {
                return Ok(await _wishListService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-wishlist")]
        public async Task<ActionResult<WishList>> Post([FromBody] WishList request)
        {
            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích của người dùng hay chưa
            var existingWishList = await _wishListService.GetAll();

            if (existingWishList.FirstOrDefault(c => c.ProductId == request.ProductId) != null)
            {
                return Ok("Sản phẩm đã có trong danh sách yêu thích");
            }

            // Nếu sản phẩm chưa tồn tại trong danh sách yêu thích, thêm sản phẩm vào danh sách
            var result = await _wishListService.Create(request.UserId, request.ProductId);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-{id}")]
        public async Task<ActionResult<WishList>> Delete(Guid id)
        {
            var result = await _wishListService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
