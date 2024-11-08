using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanTaiQuayController : Controller
	{
		private  HttpClient _httpClient;
		Uri _url;
		public BanTaiQuayController(HttpClient httpClient)
		{
			_httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7007/api/");
		}

		[HttpGet]
		public async Task<IActionResult> BanHang()
		{
            
            var listhdcho = await _httpClient.GetFromJsonAsync<List<Order>>("order/getall"); ;
			var deletehdcho = listhdcho.Where(c => !c.Create_Date.Equals(DateTime.Today.Date)).ToList();
			foreach (var item in deletehdcho)
			{
				var response = await _httpClient.DeleteAsync($"order/delete/{item.Id}");
			}
			listhdcho = await _httpClient.GetFromJsonAsync<List<Order>>("order/getall");
			ViewData["lsthdcho"] = listhdcho;
			return View();
		}
        // Sản phẩm
        [HttpGet]
        public async Task<IActionResult> LoadSp(int page, int pagesize)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<ProductVM>>("product/get-all-productvm");
            listsanPham = listsanPham.Where(c => c.MinPrice > 0).ToList();
            var model = listsanPham.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            int totalRow = listsanPham.Count;
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
    }
}
