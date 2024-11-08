using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BanQuanAo.Client.Controllers
{
    public class AdressController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdressController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> getall()
        {
            var alladress = await _httpClient.GetFromJsonAsync<List<Adress>>("https://localhost:7007/api/Adress/Get-All-Adress");
            return View(alladress);
        }

        public async Task<IActionResult> CreateAdress()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdress(Adress adress)
        {
            string userid = @User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Null";
            adress.UserId = Guid.Parse(userid);
            var creatadress = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/Adress/Insert-Adress", adress);
            if (creatadress.IsSuccessStatusCode)
            {
                return RedirectToAction("getall");
            }
            return View();
        }
        public async Task<IActionResult> detailAdress(Guid id)
        {

            var result = await _httpClient.GetFromJsonAsync<Adress>($"https://localhost:7007/api/Adress/Get-Adress-ById?id={id}");

            return View(result);
        }


        public async Task<IActionResult> editAdress(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<Adress>($"https://localhost:7007/api/Adress/Get-Adress-ById?id={id}");

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> editAdress(Adress adress)
        {
            string userid = @User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Null";
            var userids = Guid.Parse(userid);
            var result = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/Adress/Update-Adress?id={adress.Id}&UserId={userids}", adress);
            return RedirectToAction("getall");
        }

        public async Task<IActionResult> deleteAdress(Guid id)
        {
            string userid = @User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Null";
            
            var a = await _httpClient.DeleteAsync($"https://localhost:7007/api/Adress/Delete-Adress?id={id}&UserId={userid}");
            return RedirectToAction("getall");
        }

    }
}
