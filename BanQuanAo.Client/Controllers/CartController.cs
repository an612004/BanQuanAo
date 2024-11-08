using BanQuanAo.Client.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace BanQuanAo.Client.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> ShowCart()
        {

            if (User.Identity.IsAuthenticated)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userIdClaim = identity.FindFirst(ClaimTypes.Name);
                var getColor = await _httpClient.GetFromJsonAsync<List<Colors>>("https://localhost:7007/api/color/get-all-Color");
                var getCate = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
                var getSize = await _httpClient.GetFromJsonAsync<List<Size>>("https://localhost:7007/api/size/get-all-size");
                var id = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var getCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={id}");

                var listCartDetail = await _httpClient.GetFromJsonAsync<List<ViewCartDetails>>("https://localhost:7007/api/CartDetails/Get-All");
                List<ViewCartDetails>? listcartDetailbyIdCart = listCartDetail.Where(c => c.CartId == getCart.Id).ToList();
                var getAllProductDetail = await _httpClient.GetFromJsonAsync<List<ProductDetail>>("https://localhost:7007/api/productDetail/get-all-productdetail"); // lấy hết spct
                var productDetailCheck = getAllProductDetail.Where(productDetail => listcartDetailbyIdCart.Any(cartDetail => cartDetail.ProductDetail_Id == productDetail.Id && cartDetail.Quantity > productDetail.Quantity || cartDetail.ProductDetail_Id == productDetail.Id&&productDetail.Status == false) ).ToList();
                // lấy sp sao cho sl trong cart> sl kho
                var listcartDetailbyIdCartJson = JsonConvert.SerializeObject(listcartDetailbyIdCart);
                // Lưu chuỗi JSON vào TempData
                TempData["Cart"] = listcartDetailbyIdCartJson;
                ViewData["productDetailCheck"] = productDetailCheck;
                ViewData["color"] = getColor;
                ViewData["size"] = getSize;
                return View(listcartDetailbyIdCart);
            }
            else
            {

                var getColor = await _httpClient.GetFromJsonAsync<List<Colors>>("https://localhost:7007/api/color/get-all-Color");
                var getCate = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
                var getSize = await _httpClient.GetFromJsonAsync<List<Size>>("https://localhost:7007/api/size/get-all-size");
                var LstCartItem = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/CartDetails/Get-cartItemViewFromLstCartItem", LstCartItem);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var ViewCartDetails = JsonConvert.DeserializeObject<List<ViewCartDetails>>(responseData);
                    // Lưu chuỗi JSON vào TempData
                    var getAllProductDetail = await _httpClient.GetFromJsonAsync<List<ProductDetail>>("https://localhost:7007/api/productDetail/get-all-productdetail"); // lấy hết spct
                    var productDetailCheck = getAllProductDetail.Where(productDetail => ViewCartDetails.Any(cartDetail => cartDetail.ProductDetail_Id == productDetail.Id && cartDetail.Quantity > productDetail.Quantity || cartDetail.ProductDetail_Id == productDetail.Id && productDetail.Status == false) ).ToList();
                    // lấy sp sao cho sl trong cart> sl kho

                    ViewData["productDetailCheck"] = productDetailCheck;
                    ViewData["color"] = getColor;
                    ViewData["size"] = getSize;
                    return View(ViewCartDetails);
                }
                return BadRequest();
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddtoCart(string ProductName, Guid colorId, Guid sizeId, int quantity)
        {
            //

            if (quantity <= 0)
            {
                return Json(new { message = "Số lượng sản phẩm phải là số lớn hơn 0", status = false });
            }
            if (colorId == Guid.Parse("00000000-0000-0000-0000-000000000000") || sizeId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return Json(new { message = "Vui lòng chọn biến thể", status = false });
            }
            else
            {
                var prodctDetailViewModel = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail"); // get productdetail model
                var ProductDetailToAddCart = prodctDetailViewModel.FirstOrDefault(c => c.ProductName == ProductName && c.ColorId == colorId && c.SizeId == sizeId);
                if (User.Identity.IsAuthenticated) // đã đăng nhập
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value); // userId
                    var MyCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={userID}");// get my cart                 
                    var ItemInMyCart = await _httpClient.GetFromJsonAsync<List<CartItem>>($"https://localhost:7007/api/cartitem/getcartitembycartid?cartid={MyCart.Id}"); // lấy item in cart

                    // lấy được product detail để add cart, lấy được cart item
                    // check exist trong cart item
                    var checkExistInCartItem = ItemInMyCart.Where(c => c.ProductDetail_ID == ProductDetailToAddCart.Id);
                    if (checkExistInCartItem.Count() != 1) // nếu sp không tồn tại trong cart item
                    {

                        var cartItem = new CartItem()
                        {
                            ProductDetail_ID = ProductDetailToAddCart.Id,
                            CartId = MyCart.Id,
                            Price = (int)(ProductDetailToAddCart.PriceSale),
                        };
                        if (ProductDetailToAddCart.Quantity == 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        if (ProductDetailToAddCart.Quantity < quantity)
                        {
                            return Json(new { message = "Vui lòng chọn lại số lượng nhỏ hơn số lượng sản phẩm tồn!!", status = false });
                        }

                        if (ProductDetailToAddCart.Quantity <= 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        if (ProductDetailToAddCart.Status==false)
                        {
                            return Json(new { message = "Sản phẩm đang ngừng bán!!", status = false });
                        }

                        else
                        {
                            if (ProductDetailToAddCart.Quantity == 0)
                            {
                                return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                            }
                            if (ProductDetailToAddCart.Quantity < quantity)
                            {
                                return Json(new { message = "Vui lòng chọn lại số lượng nhỏ hơn số lượng sản phẩm tồn!!", status = false });
                            }
                            if (ProductDetailToAddCart.Quantity <= 0)
                            {
                                return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                            }
                            cartItem.Quantity = quantity;
                            var response = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/cartitem/Insert-Cart-Item", cartItem);
                            if (!response.IsSuccessStatusCode)
                            {
                                return Json(new { message = "Thêm sản phẩm thất bại!!", status = false });
                            }
                            return Json(new { message = "Thêm thành công vào giỏ hàng", status = true });
                        }

                    }
                    else
                    {
                        if (ProductDetailToAddCart.Quantity == 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        var productDetailInCart = checkExistInCartItem.FirstOrDefault();

                        if (ProductDetailToAddCart.Quantity < quantity + productDetailInCart.Quantity)
                        {
                            return Json(new { message = "Số lượng sản phẩm trong giỏ hàng và số lượng muốn thêm vào vượt quá số lượng tồn kho. Vui lòng giảm số lượng hoặc chọn sản phẩm khác.", status = false });
                        }
                        if (ProductDetailToAddCart.Quantity <= 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng hoặc số lượng không hợp lệ. Vui lòng chọn số lượng hợp lệ.", status = false });
                        }
                        if (ProductDetailToAddCart.Status == false)
                        {
                            return Json(new { message = "Sản phẩm đang ngừng bán!!", status = false });
                        }
                        productDetailInCart.Quantity += quantity;
                        var updateResponse = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/cartitem/Update-CartItem?id={productDetailInCart.Id}", productDetailInCart);
                        if (!updateResponse.IsSuccessStatusCode)
                        {
                            return Json(new { message = "Vui lòng chọn biến thể", status = false });
                        }
                        return Json(new { message = "Thêm thành công", status = true });
                    }

                }
                else // không đăng nhập
                {
                    var ItemInMyCart = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                    var checkExistInCartItem = ItemInMyCart.Where(c => c.ProductDetail_ID == ProductDetailToAddCart.Id);
                    if (checkExistInCartItem.Count() != 1) // nếu sp không tồn tại trong cart item
                    {

                        var cartItem = new CartItem()
                        {
                            Id = Guid.NewGuid(),
                            ProductDetail_ID = ProductDetailToAddCart.Id,
                            Price = (int)(ProductDetailToAddCart.PriceSale),
                        };
                        if (ProductDetailToAddCart.Quantity == 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        if (ProductDetailToAddCart.Quantity < quantity)
                        {
                            return Json(new { message = "Vui lòng chọn lại số lượng nhỏ hơn số lượng sản phẩm tồn!!", status = false });
                        }

                        if (ProductDetailToAddCart.Quantity <= 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        if (ProductDetailToAddCart.Status == false)
                        {
                            return Json(new { message = "Sản phẩm đang ngừng bán!!", status = false });
                        }
                        else
                        {
                            if (ProductDetailToAddCart.Quantity == 0)
                            {
                                return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                            }
                            if (ProductDetailToAddCart.Quantity < quantity)
                            {
                                return Json(new { message = "Vui lòng chọn lại số lượng nhỏ hơn số lượng sản phẩm tồn!!", status = false });
                            }
                            if (ProductDetailToAddCart.Quantity <= 0)
                            {
                                return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                            }
                            cartItem.Quantity = quantity; // cộng số lượng
                            var test1 = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                            ItemInMyCart.Add(cartItem); // thêm bản ghi mới vào cart
                            SessionServices.SetCartItemToSession(HttpContext.Session, "Cart", ItemInMyCart); // lưu vào cart
                            var test2 = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                            return Json(new { message = "Thêm thành công vào giỏ hàng", status = true });
                        }

                    }
                    else // nếu sp tồn tại trong cart
                    {
                        if (ProductDetailToAddCart.Quantity == 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng!!", status = false });
                        }
                        if (ProductDetailToAddCart.Status == false)
                        {
                            return Json(new { message = "Sản phẩm đang ngừng bán!!", status = false });
                        }
                        var productDetailInCart = checkExistInCartItem.FirstOrDefault();

                        if (ProductDetailToAddCart.Quantity < quantity + productDetailInCart.Quantity)
                        {
                            return Json(new { message = "Số lượng sản phẩm trong giỏ hàng và số lượng muốn thêm vào vượt quá số lượng tồn kho. Vui lòng giảm số lượng hoặc chọn sản phẩm khác.", status = false });
                        }
                        if (ProductDetailToAddCart.Quantity <= 0)
                        {
                            return Json(new { message = "Sản phẩm đã hết hàng hoặc số lượng không hợp lệ. Vui lòng chọn số lượng hợp lệ.", status = false });
                        }
                        // cộng số lượng
                        productDetailInCart.Quantity += quantity; // cộng số lượng
                        var test1 = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                        var products = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                        SessionServices.SetCartItemToSession(HttpContext.Session, "Cart", ItemInMyCart); // lưu vào cart
                        var test2 = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                        return Json(new { message = "Thêm thành công", status = true });
                    }
                }
            }

        }
        public async Task<IActionResult> TangSL(Guid id, Guid idCartItem, Guid idgh)
        {
            try
            {
                var url = $"https://localhost:7007/api/CartDetails/TangSl?id={id}&idCartItem={idCartItem}&idgh={idgh}";
                var response = await _httpClient.GetAsync(url);
                return RedirectToAction("ShowCart");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }
        public async Task<IActionResult> GiamSL(Guid id, Guid idCartItem, Guid idgh)
        {
            try
            {
                var url = $"https://localhost:7007/api/CartDetails/GiamSL?id={id}&idCartItem={idCartItem}&idgh={idgh}";
                var response = await _httpClient.GetAsync(url);
                return RedirectToAction("ShowCart");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }
        [HttpPost]
        public async Task<JsonResult> UpdateSLInCart(Guid idProductDetail, int newQuantity)
        {
            if (User.Identity.IsAuthenticated) // đã đăng nhập
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var MyCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={userID}");
                var ListCartItem = await _httpClient.GetFromJsonAsync<List<CartItem>>($"https://localhost:7007/api/cartitem/getcartitembycartid?cartid={MyCart.Id}");
                var cartItem = ListCartItem.FirstOrDefault(c => c.ProductDetail_ID == idProductDetail); // cartitemid
                var ProductDetail = await _httpClient.GetFromJsonAsync<ProductDetailVM>($"https://localhost:7007/api/productDetail/get/{idProductDetail}"); // sp kho
                if (ProductDetail.Quantity < newQuantity)
                {
                    return Json(new { message = "Sản phẩm vượt quá giới hạn trong kho", status = false });
                }
                if (newQuantity <= 0)
                {
                    return Json(new { message = "Sản phẩm tối thiểu là 1", status = false });
                }
                var Response = await _httpClient.GetAsync($"https://localhost:7007/api/cartitem/UpdateQuantityCartItem?cartItemId={cartItem.Id}&quantity={newQuantity}");
                if (Response.IsSuccessStatusCode)
                {
                    return Json(new { message = "OK", status = true });
                }
                return Json(new { message = "Lỗi không xác định", status = false });
            }
            else//chưa đăng nhập
            {

                var ListCartItems = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                var cartItem = ListCartItems.FirstOrDefault(c => c.ProductDetail_ID == idProductDetail);
                var ProductDetail = await _httpClient.GetFromJsonAsync<ProductDetailVM>($"https://localhost:7007/api/productDetail/get/{idProductDetail}"); // sp kho
                if (ProductDetail.Quantity < newQuantity)
                {
                    return Json(new { message = "Sản phẩm vượt quá giới hạn trong kho", status = false });
                }
                if (newQuantity <= 0)
                {
                    return Json(new { message = "Sản phẩm tối thiểu là 1", status = false });
                }
                cartItem.Quantity = newQuantity;
                SessionServices.SetCartItemToSession(HttpContext.Session, "Cart", ListCartItems);
                return Json(new { message = "OK", status = true });
            }

        }
        [HttpGet]
        public async Task<JsonResult> DeleteAllItemInCart()
        {
            try
            {
                if (User.Identity.IsAuthenticated) // đã đăng nhập
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var MyCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={userID}");
                    var url = $"https://localhost:7007/api/cartitem/Delete-CartItem?cartId={MyCart.Id}";
                    var response = await _httpClient.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { message = "Xoá giỏ hàng thành công!!!", status = true });
                    }
                    return Json(new { message = "Lỗi không xác định", status = false });
                }
                else
                {
                    var lstCartItem = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                    for (int i = lstCartItem.Count - 1; i >= 0; i--)
                    {
                        lstCartItem.RemoveAt(i);
                    }

                    SessionServices.SetCartItemToSession(HttpContext.Session, "Cart", lstCartItem);
                    return Json(new { message = "Xoá giỏ hàng thành công!!!", status = true });
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Lỗi không xác định", status = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteItemInCart(Guid ProductDetailId)
        {
            try
            {
                if (User.Identity.IsAuthenticated) // đã đăng nhập
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var MyCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={userID}");
                    var lstCartItem = await _httpClient.GetFromJsonAsync<List<CartItem>>($"https://localhost:7007/api/cartitem/getcartitembycartid?cartid={MyCart.Id}");
                    var cartItem = lstCartItem.FirstOrDefault(c => c.ProductDetail_ID == ProductDetailId);
                    var url = $"https://localhost:7007/api/CartDetails/Delete/{ProductDetailId}?idCartItem={cartItem.Id}&idgh={MyCart.Id}";
                    var response = await _httpClient.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { message = "Xoá giỏ hàng thành công!!!", status = true });
                    }
                    return Json(new { message = "Lỗi không xác định", status = false });
                }
                else
                {
                    var lstCartItem = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                    lstCartItem.RemoveAll(c => c.ProductDetail_ID == ProductDetailId);
                    SessionServices.SetCartItemToSession(HttpContext.Session, "Cart", lstCartItem);
                    return Json(new { message = "Xoá giỏ hàng thành công!!!", status = true });

                }
            }
            catch (Exception)
            {
                return Json(new { message = "Lỗi không xác định", status = false });

            }

        }

        public async Task<IActionResult> GetCartTotalItems()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                        var MyCart = await _httpClient.GetFromJsonAsync<Cart>($"https://localhost:7007/api/cart/get-item-Cart?userId={userID}");
                        if (MyCart != null)
                        {
                            var ListCartItem = await _httpClient.GetFromJsonAsync<List<CartItem>>($"https://localhost:7007/api/cartitem/getcartitembycartid?cartid={MyCart.Id}");

                            // Tính tổng số lượng của các CartItem
                            var totalItems = ListCartItem?.Count() ?? 0;

                            // Trả về tổng số lượng dưới dạng JSON
                            return Json(new { totalItems });
                        }
                    }
                    return Json(new { totalItems = 0 });
                }
                else
                {
                    var CartItems = SessionServices.GetCartItemFromSession(HttpContext.Session, "Cart");
                    return Json(new { totalItems = CartItems.Count });
                }
                // Trả về kết quả cho trường hợp chưa đăng nhập hoặc có lỗi xảy ra
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return Json(new { totalItems = 0 });
            }
        }
    }
}
