using BanQuanAo.Client.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using X.PagedList;

namespace BanQuanAo.Client.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;


        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<JsonResult> WishList(Guid ProductId)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var userIdClaim = identity.FindFirst(ClaimTypes.Name);

                    if (userIdClaim != null)
                    {
                        var userName = userIdClaim.Value;
                        var getUserbyName = await _httpClient.GetFromJsonAsync<User>($"https://localhost:7007/api/users/get/{userName}");

                        if (getUserbyName != null)
                        {
                            var userId = getUserbyName.Id;


                            // Tạo yêu cầu để thêm sản phẩm vào danh sách yêu thích
                            var requestData = new
                            {
                                UserId = userId,
                                ProductId = ProductId
                            };

                            // Gửi yêu cầu POST đến API
                            var response = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/WishList/create-wishlist", requestData);

                            // Kiểm tra xem yêu cầu có thành công không
                            if (response.IsSuccessStatusCode)
                            {
                                return Json(new { success = true });
                            }
                            else
                            {
                                // Xử lý khi có lỗi từ API khi thêm vào danh sách yêu thích
                                return Json(new { success = false, errorMessage = "Có lỗi khi thêm vào danh sách yêu thích." });
                            }
                        }
                    }
                }

                return Json(new { success = false, errorMessage = "Không thể xác định người dùng." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> ShowWishList()
        {
            var productDetail = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail");
            var productImage = await _httpClient.GetFromJsonAsync<List<ProductImage>>("https://localhost:7007/api/productimage/get-all-productimage");
            var allProducts = await _httpClient.GetFromJsonAsync<List<ProductVM>>("https://localhost:7007/api/product/get-all-productvm");
            var wishList = await _httpClient.GetFromJsonAsync<List<WishListVM>>("https://localhost:7007/api/WishList/get-all");
            allProducts = allProducts.GroupBy(p => new { p.ProductName }).Select(g => g.First()).Where(c => productDetail.Any(b => b.ProductId == c.Id)).ToList();
            // Lọc danh sách sản phẩm yêu thích dựa trên ProductId
            var wishListProducts = allProducts.Where(p => wishList.Any(w => w.ProductId == p.Id)).ToList();

            ViewData["productDetail"] = productDetail;
            ViewData["productImage"] = productImage;
            ViewData["wishList"] = wishList;

            return View(wishListProducts);
        }

        public async Task<JsonResult> DeleteFromWishList(Guid idwistlist)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7007/api/WishList/delete-{idwistlist}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { message = "Xoá sản phẩm khỏi danh sách yêu thích thành công." });
            }
            else
            {
                return Json(new { message = "Có lỗi xảy ra khi xoá sản phẩm khỏi danh sách yêu thích." });
            }
        }

        public async Task<IActionResult> Filter(string sortOrder)
        {
            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
            var productDetail = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail");
            var ProductImage = await _httpClient.GetFromJsonAsync<List<ProductImage>>("https://localhost:7007/api/productimage/get-all-productimage");

            ViewData["productCategory"] = productCategory;
            ViewData["productBrand"] = productBrand;
            ViewData["productMaterial"] = productMaterial;
            ViewData["productDetail"] = productDetail;
            ViewData["ProductImage"] = ProductImage;
            var allproduct = await _httpClient.GetFromJsonAsync<List<ProductVM>>("https://localhost:7007/api/product/get-all-productvm");
            allproduct = allproduct.GroupBy(p => new { p.ProductName }).Select(g => g.First()).Where(c => productDetail.Any(b => b.ProductId == c.Id)).ToList();
            switch (sortOrder)
            {
                case "best-selling":
                    allproduct = allproduct.OrderBy(p => p.ProductDvms?.Sum(d => d.Quantity)).ToList();
                    break;
                case "a":
                    allproduct = allproduct.OrderBy(p => p.ProductName).ToList();
                    break;
                case "high-price":
                    allproduct = allproduct.OrderByDescending(p => p.MaxPrice).ToList();
                    break;
                case "low-price":
                    allproduct = allproduct.OrderBy(p => p.MinPrice).ToList();
                    break;
                case "z":
                    allproduct = allproduct.OrderByDescending(p => p.ProductName).ToList();
                    break;
                default:
                    allproduct = allproduct.OrderBy(p => p.ProductName).ToList();
                    break;
            }
            return View("ListProduct", allproduct);
        }
        // Filters
        public async Task<List<ProductVM>> Filter(string sortOrder, List<ProductVM> lstProductVm)
        {
            switch (sortOrder)
            {
                case "a":
                    lstProductVm = lstProductVm.OrderBy(p => p.ProductName).ToList();
                    break;
                case "high-price":
                    lstProductVm = lstProductVm.OrderByDescending(p => p.MaxPrice).ToList();
                    break;
                case "low-price":
                    lstProductVm = lstProductVm.OrderBy(p => p.MinPrice).ToList();
                    break;
                case "z":
                    lstProductVm = lstProductVm.OrderByDescending(p => p.ProductName).ToList();
                    break;
                default:
                    lstProductVm = lstProductVm.OrderBy(p => p.ProductName).ToList();
                    break;
            }
            return lstProductVm;
        }
        // Search
        public async Task<List<ProductVM>> Search(string name, List<ProductVM> lstProductVm)
        {
            lstProductVm = lstProductVm.Where(p => p.ProductName.ToLower().Contains(name.ToLower())).ToList();
            return lstProductVm;
        }
        // filter by form
        public async Task<List<ProductVM>> Filter(Guid?[] SelectedCategory, Guid?[] SelectedBrand, Guid?[] SelectedMaterial, int? minPrice, int? maxPrice, string? sortOrder, List<ProductVM> lstProductVm)
        {
            if (SelectedCategory.Length > 0)
            {
                lstProductVm = lstProductVm.FindAll(c => SelectedCategory.Contains(c.CategoryId)).ToList();
            }
            if (SelectedBrand.Length > 0)
            {
                lstProductVm = lstProductVm.FindAll(c => SelectedBrand.Contains(c.BrandId)).ToList();
            }
            if (SelectedMaterial.Length > 0)
            {
                lstProductVm = lstProductVm.FindAll(c => SelectedMaterial.Contains(c.MaterialId)).ToList();
            }
            if (minPrice != null && minPrice != 0 && maxPrice != null && maxPrice != 0)
            {
                if (minPrice > maxPrice)
                {
                    int? temp = minPrice;
                    minPrice = maxPrice;
                    maxPrice = temp;
                }
                lstProductVm = lstProductVm.Where(p =>
            (minPrice >= p.MinPrice && minPrice <= p.MaxPrice) ||
            (maxPrice >= p.MinPrice && maxPrice <= p.MaxPrice))
        .ToList();
            }
            if (sortOrder != null)
            {
                lstProductVm = await Filter(sortOrder, lstProductVm);
            }
            return lstProductVm;
        }


        public async Task<IActionResult> ListProductAsync(string? SearchString, string? sortOrder, Guid?[] SelectedCategory, Guid?[] SelectedBrand, Guid?[] SelectedMaterial, int? minPrice, int? maxPrice, int? page)
        {
            if (page == null) page = 1;
            //chỉnh số lượng sp hiển thị ở đây
            //pagesize bằng 1 là hiển thị 1sp 1 trang á
            int pageSize = 9;// số sp trên 1 trang
            int pageNumber = (page ?? 1);

            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
            var productDetail = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail");
            var ProductImage = await _httpClient.GetFromJsonAsync<List<ProductImage>>("https://localhost:7007/api/productimage/get-all-productimage");

            List<SelectListItem> selectListItemsProductCategory = productCategory.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.CategoryName
            }).ToList();
            List<SelectListItem> selectListItemsProductMaterial = productMaterial.Select(material => new SelectListItem
            {
                Value = material.Id.ToString(),
                Text = material.MaterialName
            }).ToList();
            List<SelectListItem> selectListItemsProductBrand = productBrand.Select(brand => new SelectListItem
            {
                Value = brand.Id.ToString(),
                Text = brand.BrandName
            }).ToList();

            ViewData["productCategory"] = selectListItemsProductCategory;
            ViewData["productBrand"] = selectListItemsProductBrand;
            ViewData["productMaterial"] = selectListItemsProductMaterial;
            ViewData["productDetail"] = productDetail;
            ViewData["ProductImage"] = ProductImage;
            ViewBag.SearchString = SearchString;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.SelectedMaterial = SelectedMaterial;
            ViewBag.SelectedBrand = SelectedBrand;
            ViewBag.SelectedCategory = SelectedCategory;
            ViewBag.sortOrder = sortOrder;

            var allproduct = await _httpClient.GetFromJsonAsync<List<ProductVM>>("https://localhost:7007/api/product/get-all-productvm");
            allproduct = allproduct.GroupBy(p => new { p.ProductName }).Select(g => g.First()).Where(c => productDetail.Any(b => b.ProductId == c.Id && c.AvailableQuantity > 0 && b.Status == true) && c.Status == true).ToList();
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                allproduct = await Search(SearchString, allproduct);
            }
            allproduct = await Filter(SelectedCategory, SelectedBrand, SelectedMaterial, minPrice, maxPrice, sortOrder, allproduct);
            return View(allproduct.ToPagedList(pageNumber, pageSize));
        }
        public async Task<IActionResult> ProductDetailAsync(Guid id)
        {
            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var allproduct = await _httpClient.GetFromJsonAsync<List<ProductVM>>("https://localhost:7007/api/product/get-all-productvm");
            var allproductDetail = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail");
            var allProductImage = await _httpClient.GetFromJsonAsync<List<ProductImage>>("https://localhost:7007/api/productimage/get-all-productimage");
            var Product = allproduct.FirstOrDefault(x => x.Id == id);
            allproduct = allproduct.GroupBy(p => new { p.ProductName }).Select(g => g.First()).Where(c => allproductDetail.Any(b => b.ProductId == c.Id)).ToList();
            var productdetail = allproductDetail.FirstOrDefault(c => c.ProductId == Product.Id);
            var lstProductImage = allProductImage.Where(c => c.ProductDetailId == productdetail.Id).ToList();
            var idCate = Product.CategoryId;
            var relatedProducts = allproduct
                .Where(p => p.Id != id && p.CategoryId == idCate)
                .Take(4) // Lấy 4 sản phẩm liên quan (hoặc số lượng mong muốn)
                .ToList();
            ViewData["productDetail"] = allproductDetail;
            ViewData["lstProductImage"] = lstProductImage;
            ViewData["lstProductImage1"] = allProductImage;
            ViewData["lstCate"] = relatedProducts;
            //ViewData["lstallproduct"] = allproduct;
            return View(Product);

        }

        public async Task<JsonResult> GetProductDetail(Guid productId, Guid sizeId, Guid colorId)
        {

            try
            {
                var productdetail = await _httpClient.GetFromJsonAsync<ProductDetail>($"https://localhost:7007/api/productDetail/GetProductDetail?productId={productId}&sizeId={sizeId}&colorId={colorId}");
                return Json(new { message = "OK", status = true, quantity = productdetail.Quantity, price = productdetail.PriceSale });
            }
            catch (Exception)
            {

                return Json(new { message = "Không có size và màu bạn chọn :(", status = false });

            }
        }
    }
}
