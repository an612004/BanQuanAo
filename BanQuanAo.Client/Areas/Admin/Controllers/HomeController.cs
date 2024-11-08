using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string startDate, string endDate)
        {
            try
            {
                if (startDate == null || endDate == null)
                {
                    startDate = DateTime.Now.AddDays(-7).ToString();
                    endDate = DateTime.Now.ToString();
                }
                var response = await _httpClient.GetAsync("https://localhost:7007/api/ThongKe/ThongKe?startDate=" + startDate + "&endDate=" + endDate);
                var lst = JsonConvert.DeserializeObject<ThongKeViewModel>(response.Content.ReadAsStringAsync().Result);
                return View(lst);
            }
            catch
            {
                return View(new ThongKeViewModel());
            }
        }
    }
}
