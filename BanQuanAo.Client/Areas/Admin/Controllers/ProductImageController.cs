using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductImageController : Controller
    {

        private HttpClient _httpClient;
        Uri Url = new Uri("https://localhost:7007/api/productimage");
        public ProductImageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Show()
        {
            var response = await _httpClient.GetAsync(Url + "/get-all-productimage");
            // Lấy dữ liệu Json trả về từ Api được call dạng string
            string apiData = await response.Content.ReadAsStringAsync();
            // Lấy kqua trả về từ API
            // Đọc từ string Json vừa thu được sang List<T>
            var colors = JsonConvert.DeserializeObject<List<ProductImage>>(apiData);
            return View(colors);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImage images, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImagesProduct", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                images.ImageUrl = fileName;
            }
            //string url =
            //    $"https://localhost:7007/api/productimage/create-productimage?imageUrl={images.ImageUrl}&ProductDetailId={images.ProductDetailId}";
            //var json = JsonConvert.SerializeObject(images);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsJsonAsync(Url + "/create-productimage", images);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }

        }
    }
}
