using AspNetCoreHero.ToastNotification.Abstractions;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        public INotyfService _notyf;

        public ProductController(HttpClient httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }


        // GET: ProductController
        public async Task<IActionResult> GetAllProduct()
        {
            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");

            ViewData["productCategory"] = productCategory;
            ViewData["productBrand"] = productBrand;
            ViewData["productMaterial"] = productMaterial;
            var allproduct = await _httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7007/api/product/get-all-product");
            return View(allproduct);
        }
    

        // GET: ProductController/Create
        public async Task<IActionResult> CreateProduct()
        {
            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
            ViewData["productCategory"] = productCategory;
            ViewData["productBrand"] = productBrand;
            ViewData["productMaterial"] = productMaterial;

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product pro, string edit)
        {
            try
            {
                var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
                var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
                var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
                ViewData["productCategory"] = productCategory;
                ViewData["productBrand"] = productBrand;
                ViewData["productMaterial"] = productMaterial;
                if (pro.Description == null ||  edit == null)
                {
                    _notyf.Warning("Không được để trống!");
                }
                var allproduct = await _httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7007/api/product/get-all-product");
                if ( pro.ProductName != null )
                {
                    var timkiem = allproduct.FirstOrDefault(x => x.ProductName.Trim().ToLower() == pro.ProductName.Trim().ToLower());
                    if (timkiem != null)
                    {
                        _notyf.Warning("Đã có sản phẩm này!");
                        return View();
                    }
                    else
                    {
                        pro.Status = true;
                        pro.Long_Description = edit;
                        pro.Create_At = DateTime.Now;
                        pro.AvailableQuantity = 0;
                        pro.Update_At = null;
                        var createpro = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/product/create-product", pro);
                        if (createpro.IsSuccessStatusCode)
                        {
                            _notyf.Success("Thêm thành công!");
                            return RedirectToAction("GetAllProduct");
                        }
                    }
                    
                }
                return RedirectToAction("CreateProduct");
            }
            catch
            {
                _notyf.Error("Lỗi!");
                return View();
            }
        }
        
        public async Task<IActionResult> EditProduct(Guid id)
        {
            var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");

            ViewData["productCategory"] = productCategory;
            ViewData["productBrand"] = productBrand;
            ViewData["productMaterial"] = productMaterial;
            var result = await _httpClient.GetFromJsonAsync<Product>($"https://localhost:7007/api/product/get-{id}");

            return View(result);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product pro, string edit)
        {
            try
            {
                var productCategory = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
                var productBrand = await _httpClient.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
                var productMaterial = await _httpClient.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
                ViewData["productCategory"] = productCategory;
                ViewData["productBrand"] = productBrand;
                ViewData["productMaterial"] = productMaterial;
                if (pro.Description == null || edit == null)
                {
                    _notyf.Warning("Không được để trống!");
                    //return View(pro);
                }
                else
                {
                    pro.Long_Description = edit;
                    pro.Update_At = DateTime.Now;
                    pro.Status = true;
                    var result = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/product/update-product-{pro.Id}", pro);
                    if (result.IsSuccessStatusCode)
                    {
                        _notyf.Success("Sửa thành công!");
                    }
                    return RedirectToAction("GetAllProduct");
                }
                return View();
            }
            catch
            {
                _notyf.Error("Lỗi!");
                return View();
            }
        }

        // POST: ProductController/Delete/5
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7007/api/product/delete-product-{id}");
            return RedirectToAction("GetAllProduct");
        }
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var AllProductDetail = await _httpClient.GetFromJsonAsync<List<ProductDetailVM>>("https://localhost:7007/api/productDetail/get-all-productdetail");
            var ProductDetailInProduct = AllProductDetail.Where(c=>c.ProductId==id).ToList();
            return View(ProductDetailInProduct);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatusAsync(Guid idsp, bool status)
        {

            var response = await _httpClient.GetAsync($"https://localhost:7007/api/product/ChangeStatus?idsp={idsp}&status={status}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Cập nhật thành công");
                return RedirectToAction("GetAllProduct");
            }
            else
            {
                _notyf.Error("Lỗi");
                return View();
            }
        }

    }
}
