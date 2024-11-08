using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/productDetail")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private IProductDetailService _iproductDetailService;
        private IProductService _iproductService;

        public ProductDetailController(IProductDetailService iproductDetailService, IProductService iproductService)
        {
            _iproductDetailService = iproductDetailService;
            _iproductService = iproductService;
        }
        [HttpGet("get-all-productdetail")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iproductDetailService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("GetAllAnhSanPham")]
        public List<ProductImageVM> GetAllAnhSanPham(Guid idSanPham)
        {
            return _iproductDetailService.GetAllAnhSanPham(idSanPham);
        }
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProductDetailVM>> Get(Guid id)
        {
            try
            {
                return Ok(await _iproductDetailService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-productdetail")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDetailVM>> Post([FromForm] ProductDetailVM obj)
        {
            var result = await _iproductDetailService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPost("create-many-productdetail")]
        public async Task<ActionResult<ProductDetailVM>> PostMany([FromBody] List<ProductDetailVM> obj)
        {
            var result = await _iproductDetailService.CreateMany(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-productdetail-{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDetailVM>> Put(Guid id, [FromForm] ProductDetailVM obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            obj.Id = id;
            var result = await _iproductDetailService.Update(obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-productdetail-{id}")]
        public async Task<ActionResult<ProductDetailVM>> Delete(Guid id)
        {
            var result = await _iproductDetailService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpGet("getpriceforproductD")]
        public IActionResult GetPriceForProductD([FromQuery] Guid productId, [FromQuery] Guid sizeId, [FromQuery] Guid colorId)
        {
            var price = _iproductDetailService.GetPriceForProductDetail(productId, sizeId, colorId);
            return Ok(price);
        }
        [HttpGet("GetProductDetail")]
        public IActionResult GetProductDetail([FromQuery] Guid productId, [FromQuery] Guid sizeId, [FromQuery] Guid colorId)
        {
            var price = _iproductDetailService.GetProductDetail(productId, sizeId, colorId);
            return Ok(price);
        }

        [HttpGet("UpdateQuantityById")]
        public async Task<IActionResult> UpdateQuantityById(Guid productDetailId, int quantity)
        {
            var productdetail = await _iproductDetailService.UpdateQuantityById(productDetailId, quantity);
            var updateslProduct = await _iproductService.UpdateSLTheoSPCT();
            return Ok(productdetail);
        }
        [HttpGet("UpdateQuantityOrderFail")]
        public async Task<IActionResult> UpdateQuantityOrderFail(Guid productDetailId, int quantity)
        {
            var productdetail = await _iproductDetailService.UpdateQuantityOrderFail(productDetailId, quantity);
            var updateslProduct = await _iproductService.UpdateSLTheoSPCT();
            return Ok(productdetail);
        }
        [HttpGet("ChangeStatus")]
        public async Task<IActionResult> ChangeStatusAsync(Guid idspct, bool status)
        {
            var productdetail = await _iproductDetailService.ChangeStatusAsync( idspct, status);
            var updateslProduct = await _iproductService.UpdateSLTheoSPCT();
            return Ok(productdetail);
        }

    }
}
