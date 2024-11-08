using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/productimage")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private IProductImageService _iproductImageService;

        public ProductImageController(IProductImageService iproductImageService)
        {
            _iproductImageService = iproductImageService;
        }
        [HttpGet("get-all-productimage")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iproductImageService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<ProductImage>> Get(Guid id)
        {
            try
            {
                return Ok(await _iproductImageService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-productimage")]
        public async Task<ActionResult<ProductImageVM>> Post( ProductImageVM obj)
        {
            var result = await _iproductImageService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPost("create-many-productimage")]
        public async Task<ActionResult<ProductImageVM>> PostMany([FromBody] List<ProductImageVM> obj)
        {
            var result = await _iproductImageService.CreateMany(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-productimage-{id}")]
        public async Task<ActionResult<ProductImageVM>> Put(Guid id, [FromBody] ProductImageVM obj)
        {
            var result = await _iproductImageService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-productimage-{id}")]
        public async Task<ActionResult<ProductVM>> Delete(Guid id)
        {
            var result = await _iproductImageService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
