using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly HttpClient _client;
        public BrandController(HttpClient client)
        {
            _client = client;
        }
        // GET: BrandController
        public async Task<IActionResult> GetAllBrand()
        {
            var allBrand = await _client.GetFromJsonAsync<List<Brand>>("https://localhost:7007/api/brand/getall");
            return View(allBrand);
        }

        // GET: BrandController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var getBrand = await _client.GetFromJsonAsync<Brand>($"https://localhost:7007/api/brand/get{id}");
            return View(getBrand);
        }

        // GET: BrandController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand br)
        {
            try
            {
                var createBrand = await _client.PostAsJsonAsync("https://localhost:7007/api/brand/create", br);
                return RedirectToAction("GetAllBrand");
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var getBrand = await _client.GetFromJsonAsync<Brand>($"https://localhost:7007/api/brand/get{id}");
            return View(getBrand);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand br)
        {
            try
            {
                var editBrand = await _client.PutAsJsonAsync($"https://localhost:7007/api/brand/update{br.Id}", br);
                return RedirectToAction("GetAllBrand");
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteBrand = await _client.DeleteAsync($"https://localhost:7007/api/brand/delete{id}");
            return RedirectToAction("GetAllBrand");
        }
    }
}
