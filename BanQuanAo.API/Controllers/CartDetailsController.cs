using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailsController : ControllerBase
    {
        private ICartItemService _cartItemService;
        private IProductDetailService _productDetailService;
        private IProductImageService _productImageService;
        private IColorService _colorService;
        private IProductService _productService;
        private ICategoryService _categoryService;
        private ISizeService _sizeService;
        private IVoucherService _voucherService;

        public CartDetailsController(ICartItemService cartItemService, IProductDetailService productDetailService, IProductImageService productImageService, IColorService colorService, IProductService productService, ICategoryService categoryService, ISizeService sizeService)
        {
            _cartItemService = cartItemService;
            _productDetailService = productDetailService;
            _productImageService = productImageService;
            _colorService = colorService;
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
        }
        [HttpGet]
        [Route("Get-All")]
        public async Task<IActionResult> GetAllCartDetail()
        {
            var cartItem = await _cartItemService.GetAll();
            var productDetails = await _productDetailService.GetAll();
            var productImage = await _productImageService.GetAll();
            var color = await _colorService.GetAll();
            var cate = await _categoryService.GetAll();
            var product = await _productService.GetAll();
            var size = await _sizeService.GetAll();
            List<ViewCartDetails> view = new List<ViewCartDetails>();
            foreach (var item in cartItem)
            {
                if (cartItem.Count == 0)
                {
                    return new OkObjectResult(new { message = "Không có sản phẩm nào trong giỏ hàng. <a href='/'>Quay lại trang chủ</a>", error = -1, data = view });
                }
                var prd = productDetails.FirstOrDefault(x => x.Id == item.ProductDetail_ID);
                var pr = product.FirstOrDefault(x => x.Id == prd.ProductId);
                var ha = productImage.First(x => x.ProductDetailId == prd.Id);
                var cl = color.FirstOrDefault(x => x.ColorId == prd?.ColorId);
                var ct = cate.FirstOrDefault(x => x.Id == pr.CategoryId);
                var sz = size.FirstOrDefault(x => x.Id == prd.SizeId);
                ViewCartDetails cartDetails = new ViewCartDetails()
                {
                    Id = item.Id,
                    ImageName = ha.ImageUrl,
                    ProductName = pr.ProductName,
                    Price = prd.Price,
                    PriceSale = prd.PriceSale,
                    Quantity = item.Quantity,
                    CategoryId = ct.Id,
                    CartId = item.CartId,
                    ProductDetail_Id = prd.Id,
                    SizeId = sz.Id,
                    ColorsId = cl.ColorId,
                    TotalPrice = Convert.ToInt32(prd.PriceSale) * Convert.ToInt32(item.Quantity)
                };
                view.Add(cartDetails);
            }
            return Ok(view);
        }

        [HttpPost]
        [Route("Get-cartItemViewFromLstCartItem")]
        public async Task<IActionResult> GetAllCartDetail(List<CartItem> lstCartItem)
        {
            var cartItem = lstCartItem;
            var productDetails = await _productDetailService.GetAll();
            var productImage = await _productImageService.GetAll();
            var color = await _colorService.GetAll();
            var cate = await _categoryService.GetAll();
            var product = await _productService.GetAll();
            var size = await _sizeService.GetAll();
            List<ViewCartDetails> view = new List<ViewCartDetails>();
            foreach (var item in cartItem)
            {
                if (cartItem.Count == 0)
                {
                    return new OkObjectResult(new { message = "Không có sản phẩm nào trong giỏ hàng. <a href='/'>Quay lại trang chủ</a>", error = -1, data = view });
                }
                var prd = productDetails.FirstOrDefault(x => x.Id == item.ProductDetail_ID);
                var pr = product.FirstOrDefault(x => x.Id == prd.ProductId);
                var ha = productImage.First(x => x.ProductDetailId == prd.Id);
                var cl = color.FirstOrDefault(x => x.ColorId == prd?.ColorId);
                var ct = cate.FirstOrDefault(x => x.Id == pr.CategoryId);
                var sz = size.FirstOrDefault(x => x.Id == prd.SizeId);
                ViewCartDetails cartDetails = new ViewCartDetails()
                {
                    Id = item.Id,
                    ImageName = ha.ImageUrl,
                    ProductName = pr.ProductName,
                    Price = prd.Price,
                    PriceSale = prd.PriceSale,
                    Quantity = item.Quantity,
                    CategoryId = ct.Id,
                    CartId = item.CartId,
                    ProductDetail_Id = prd.Id,
                    SizeId = sz.Id,
                    ColorsId = cl.ColorId,
                    TotalPrice = Convert.ToInt32(prd.PriceSale) * Convert.ToInt32(item.Quantity)
                };
                view.Add(cartDetails);
            }
            return Ok(view);
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetCartDetailById(Guid id)
        {
            var result = await _cartItemService.GetCartItemsByCartIds(id);
            if (result == null) return Ok("Không tìm thấy giỏ hàng chi tiết");
            return Ok(result);
        }
        [HttpGet]
        [Route("TangSl/")]
        public async Task<IActionResult> TangSl(Guid id, Guid idCartItem, Guid idgh)
        {
            try
            {
                var result = await _cartItemService.GetAllCartItemsByCartId(idgh);
                var lstSP = await _productDetailService.GetAll();
                var respon = result.FirstOrDefault(x => x.CartId == idgh && x.ProductDetail_ID == id && x.Id == idCartItem);

                var kc = lstSP.FirstOrDefault(x => x.Id == id);
                respon.Quantity++;
                if (respon.Quantity > kc.Quantity)
                {
                    return new OkObjectResult(new { error = -1, message = "Sản phẩm tạm thời không còn" });
                }
                else
                {
                    var up = await _cartItemService.UpdateQuantityCartItem(respon);
                    if (up != null)
                    {
                        return new OkObjectResult(new { error = 0, message = "Cập nhập số lượng sản phẩm thành công" });
                    }
                    else
                    {
                        return new OkObjectResult(new { error = -2, message = "Cập nhập số lượng sản phẩm thất bại" });
                    }
                }
            }
            catch (Exception)
            {
                return new OkObjectResult(new { error = -2, message = "Cập nhập số lượng sản phẩm thất bại" });
                throw;
            }

        }
        [HttpGet]
        [Route("GiamSL/")]
        public async Task<IActionResult> GiamSL(Guid id, Guid idCartItem, Guid idgh)
        {
            try
            {
                var result = await _cartItemService.GetAllCartItemsByCartId(idgh);
                var respon = result.FirstOrDefault(x => x.CartId == idgh && x.ProductDetail_ID == id && x.Id == idCartItem);
                --respon.Quantity;
                if (respon.Quantity <= 0)
                {
                    //var up = await _cartItemService.DeleteCartItem(idCartItem, id,idgh);
                    return new OkObjectResult(new { error = -1, message = "Sản phẩm tạm thời không còn" });
                }
                else
                {
                    var up = await _cartItemService.UpdateQuantityCartItem(respon);
                    if (up != null)
                    {
                        return new OkObjectResult(new { error = 0, message = "Cập nhập số lượng sản phẩm thành công" });
                    }
                    else
                    {
                        return new OkObjectResult(new { error = -2, message = "Cập nhập số lượng sản phẩm thất bại" });
                    }
                }
            }
            catch (Exception)
            {
                return new OkObjectResult(new { error = -1, message = "Có lỗi xảy ra. Cập nhập số lượng sản phẩm thất bại" });
            }

        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateCartDetails(InsertCartDetails insertCart)
        {

            try
            {
                CartItem nv = new CartItem()
                {
                    Id = Guid.NewGuid(),
                    CartId = insertCart.CartId,
                    ProductDetail_ID = insertCart.ProductDetail_ID,
                    Quantity = insertCart.Quantity,
                    Price = insertCart.Price,
                };
                var result = await _cartItemService.AddCartItem(nv);
                return new OkObjectResult(new { message = "Thêm vào giỏ hàng thành công", error = 0 });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { message = "Có lỗi xảy ra hãy thử lại sau đó.", error = -2 });
            }

        }
        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateCartDetails(Guid id, int Quantity, int PriceSale)
        {
            var result = await _cartItemService.GetCartItemsByCartIds(id);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Không tìm thấy giỏ hàng chi tiết");
            }
            else
            {
                result.Quantity = Quantity;
                result.Price = PriceSale;
                try
                {
                    await _cartItemService.UpdateCartItem(id, Quantity, PriceSale);
                    return new OkObjectResult(new { message = "Cập nhập giỏ hàng thành công", error = 0 });
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(new { message = "Có lỗi xảy ra hãy thử lại sau đó.", error = -2 });
                }


            }

        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteItemInCart(Guid id, Guid idCartItem, Guid idgh)
        {
            try
            {
                var result = await _cartItemService.GetAllCartItemsByCartId(idgh);
                var respon = result.FirstOrDefault(x => x.CartId == idgh && x.ProductDetail_ID == id && x.Id == idCartItem);
                if (respon != null)
                {
                    var delete = await _cartItemService.Delete1Item(respon.Id);
                    if (delete)
                    {
						return new OkObjectResult(new { error = 1, message = "Sản phẩm đã được xóa" });
					}
                }
                return new OkObjectResult(new { error = -1, message = "Có lỗi xảy ra. Cập nhập số lượng sản phẩm thất bại" });
            }
            catch (Exception)
            {
                return new OkObjectResult(new { error = -1, message = "Có lỗi xảy ra. Cập nhập số lượng sản phẩm thất bại" });
            }

        }
    }
}
