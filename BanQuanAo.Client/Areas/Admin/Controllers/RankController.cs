using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RankController : Controller
    {
        private readonly HttpClient _httpClient;

        public RankController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult>Index()
        {
            var ranks = await _httpClient.GetFromJsonAsync<List<Rank>>("https://localhost:7007/api/ranks/get-ranks");
            return View(ranks);
        }
        public async Task<IActionResult> CreateRank(string rankName, string description, int pointMin, int pointMax)
        {
            var rank = new Rank()
            {
                Id = Guid.NewGuid(),
                Name = rankName,
                PointsMin = pointMin,
                PoinsMax = pointMax,
                Description = description,
            };
            var creatvoucher = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/ranks/create-rank", rank);
            if (creatvoucher.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
		[HttpPost]
        public async Task<IActionResult> EditRank(Rank rank)
        {
            string url = $"https://localhost:7007/api/ranks/update-rank/{rank.Id}";
            var result = await _httpClient.PutAsJsonAsync(url, rank);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> EditRank(Guid id)
        {
            string url = $"https://localhost:7007/api/ranks/get/{id}";
            var rank = await _httpClient.GetFromJsonAsync<Rank>(url);
            return View(rank);
        }

        // lấy id rank
        // lấy rank
        // truyền vào biến
        // đẩy biến vào session
        // lấy từ session
        // đẩy lại vào form
        // truyền lên action
        public async Task<IActionResult> DeleteRank(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7007/api/ranks/delete-rank/{id}");
            return RedirectToAction("Index");
        }
    }
}
