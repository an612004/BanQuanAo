using AspNetCoreHero.ToastNotification.Abstractions;
using BanQuanAo.Client.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductDetailController : Controller
    {
        private IproductDetailApiClient _apiClient;
        private HttpClient _httpClient;
        public INotyfService _notyf;
        public ProductDetailController(IproductDetailApiClient iproductDetail, HttpClient httpClient, INotyfService notyf)
        {
            _apiClient = iproductDetail;
            _httpClient = httpClient;
            _notyf = notyf;
        }

        public async Task<IActionResult> Show()
        {
            var response = await _apiClient.GetAllProductDetail();
            
            return View(response);

        }
        public async Task<IActionResult> Create(Guid productId)
        {
            //var productprops = _apiClient.GetListProduct();
            //ViewBag.ProductProp = productprops.Result.Select(x => new SelectListItem()
            //{
            //    Text = x.ProductName,
            //    Value = x.Id.ToString(),
            //    Selected = productId.ToString() == x.Id.ToString()
            //});
            ViewBag.productId = productId;
            var sizes = _apiClient.GetListSize();
            ViewBag.Size = sizes.Result.Select(x => new SelectListItem()
            {
                Text = x.SizeName,
                Value = x.Id.ToString()
            });
            var colors = _apiClient.GetListColor();
            ViewBag.Color = colors.Result.Select(x => new SelectListItem()
            {
                Text = x.ColorName,
                Value = x.ColorId.ToString()
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductDetailVM create, Guid productId, Guid sizeId, Guid colorId)
        {
            var productprops = _apiClient.GetListProduct();
            ViewBag.ProductProp = productprops.Result.Select(x => new SelectListItem()
            {
                Text = x.ProductName,
                Value = x.Id.ToString(),
                Selected = productId.ToString() == x.Id.ToString()
            });
            var sizes = _apiClient.GetListSize();
            ViewBag.Size = sizes.Result.Select(x => new SelectListItem()
            {
                Text = x.SizeName,
                Value = x.Id.ToString(),
                Selected = sizeId.ToString() == x.Id.ToString()
            });
            var colors = _apiClient.GetListColor();
            ViewBag.Color = colors.Result.Select(x => new SelectListItem()
            {
                Text = x.ColorName,
                Value = x.ColorId.ToString(),
                Selected = colorId.ToString() == x.ColorId.ToString()
            });
            if (create.Quantity == null || create.PriceSale == null || create.Price == null || create.filecollection == null || create.Description == null)
            {
                _notyf.Warning("Không để trống");
                return View(create);
            }
            if (create.Quantity <= 0 || create.Price < 0 || create.PriceSale < 0)
            {
                _notyf.Warning("Số lượng không âm");
                return View(create);
            }
            if (create.Quantity > 3000)
            {
                _notyf.Warning("Số lượng vượt quá kho chứa");
                return View(create);
            }
            if (create.PriceSale < create.Price )
            {
                _notyf.Warning("Giá bán phải lớn hơn giá nhập");
                return View(create);
            }

            if (create.PriceSale > 2000000000 || create.PriceSale < 0)
            {
                _notyf.Warning("Kiểm tra lại giá bán :(");
                return View(create);
            }
            create.Status = true;
            create.Create_At = DateTime.Now;
            var result = await _apiClient.CreateProduct(create, productId, sizeId, colorId);
            if (result != null)
            {
                _notyf.Success("Thêm thành công!");
                return Redirect("/Admin/Product/GetAllProduct");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id, Guid sizeId, Guid colorId)
        {
            var sizes = _apiClient.GetListSize();
            ViewBag.Size = sizes.Result.Select(x => new SelectListItem()
            {
                Text = x.SizeName,
                Value = x.Id.ToString(),
                Selected = sizeId.ToString() == x.Id.ToString()
            });
            var colors = _apiClient.GetListColor();
            ViewBag.Color = colors.Result.Select(x => new SelectListItem()
            {
                Text = x.ColorName,
                Value = x.ColorId.ToString(),
                Selected = colorId.ToString() == x.ColorId.ToString()
            });
            var response = await _apiClient.GetByIdProductDetail(id);
            var producti = await _httpClient.GetFromJsonAsync<List<ProductImageVM>>($"https://localhost:7007/api/productDetail/GetAllAnhSanPham?idSanPham={id}");
            ViewData["ProductImage"] = producti;
            return View(response);
        }

        public async Task<IActionResult> Update(ProductDetailVM create, Guid sizeId, Guid colorId)
        {
            try
            {
                var sizes = _apiClient.GetListSize();
                ViewBag.Size = sizes.Result.Select(x => new SelectListItem()
                {
                    Text = x.SizeName,
                    Value = x.Id.ToString(),
                    Selected = sizeId.ToString() == x.Id.ToString()
                });
                var colors = _apiClient.GetListColor();
                ViewBag.Color = colors.Result.Select(x => new SelectListItem()
                {
                    Text = x.ColorName,
                    Value = x.ColorId.ToString(),
                    Selected = colorId.ToString() == x.ColorId.ToString()
                });
                if (create.Quantity == null || create.PriceSale == null || create.Description == null || create.SizeId == null || create.ColorId == null)
                {
                    _notyf.Warning("Không để trống");
                    return View(create);
                }
                if (create.Quantity <= 0)
                {
                    _notyf.Warning("Số lượng không âm");
                    return View(create);
                }
                if (create.Quantity > 3000)
                {
                    _notyf.Warning("Số lượng vượt quá kho chứa");
                    return View(create);
                }
                if (create.PriceSale < create.Price)
                {
                    _notyf.Warning("Giá bán phải lớn hơn giá nhập");
                    return View(create);
                }

                if (create.PriceSale > 2000000000 || create.PriceSale <0)
                {
                    _notyf.Warning("Kiểm tra lại giá bán :(");
                    return View(create);
                }
                create.Status = true;
                create.Update_At = DateTime.Now;
                var response = await _apiClient.UpdateProduct(create, sizeId, colorId);
                
                if (response)
                {
                    _notyf.Success("Sửa thành công");
                    return Redirect("/Admin/Product/GetAllProduct");
                }
                else
                {
                    return Redirect("/Admin/Product/GetAllProduct");

                }

            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {

            var result = await _apiClient.GetByIdProductDetail(Id);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Remove(Guid Id)
        {
            var response = await _apiClient.DeleteProductDetail(Id);

            if (response)
            {
                return Redirect("/Admin/Product/GetAllProduct");

            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> ShowProI(Guid IdPr)
        {
            try
            {
                var response = _httpClient.GetAsync($"https://localhost:7007/api/productDetail/GetAllAnhSanPham?idSanPham={IdPr}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var lstAnh = JsonConvert.DeserializeObject<List<ProductImageVM>>(response.Content.ReadAsStringAsync().Result);
                    ViewData["IDSanPham"] = IdPr.ToString();
                    return View("ShowProI", lstAnh.OrderBy(x => x.ImageUrl));
                }
                else return View("ShowProI", new List<ProductImageVM>());
            }
            catch
            {
                return View("ShowProI", new List<ProductImageVM>());
            }
        }
        [HttpPost]
        public IActionResult CreateImg(IFormFile file, string idSanPham)
        {
            var anh = new ProductImageVM() { Id = Guid.NewGuid(), ImageFile = file, ProductDetailId = new Guid(idSanPham)};
       
            var create =  _httpClient.PostAsJsonAsync("https://localhost:7007/api/productimage/create-productimage", anh).Result;

            if (create.IsSuccessStatusCode)
            {
                _notyf.Success("Thêm thành công");
                // Chuyển hướng và truyền IdPr trong URL
                return RedirectToAction("ShowProI", new { idSanPham });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProI(Guid Id)
        {
        
            var response = await _httpClient.DeleteAsync($"https://localhost:7007/api/productimage/delete-productimage-{Id}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Xoá thành công");
                return RedirectToAction("Show");
            }
            else
            {
                _notyf.Error("Lỗi");
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatusAsync(Guid idspct, bool status)
        {
        
            var response = await _httpClient.GetAsync($"https://localhost:7007/api/productDetail/ChangeStatus?idspct={idspct}&status={status}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Cập nhật thành công");
                return Redirect("/Admin/Product/GetAllProduct");
            }
            else
            {
                _notyf.Error("Lỗi");
                return View();
            }
        }

    }
}
