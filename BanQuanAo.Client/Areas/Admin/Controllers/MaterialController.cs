using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MaterialController : Controller
    {
        private readonly HttpClient _client;
        public MaterialController(HttpClient client)
        {
            _client = client;   
        }
        // GET: MaterialController
        public async Task<IActionResult> GetAllMaterial()
        {
            var allMaterial = await _client.GetFromJsonAsync<List<Material>>("https://localhost:7007/api/Material/getall");
            return View(allMaterial);
        }

        // GET: MaterialController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var getMaterial = await _client.GetFromJsonAsync<Material>($"https://localhost:7007/api/Material/get-{id}");
            return View(getMaterial);
        }

        // GET: MaterialController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: MaterialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material ct)
        {
            try
            {
                var createMaterial = await _client.PostAsJsonAsync("https://localhost:7007/api/Material/create", ct);
                return RedirectToAction("GetAllMaterial");
            }
            catch
            {
                return View();
            }
        }

        // GET: MaterialController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var getMaterial = await _client.GetFromJsonAsync<Material>($"https://localhost:7007/api/Material/get-{id}");
            return View(getMaterial);
        }

        // POST: MaterialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Material ct)
        {
            try
            {
                var editMaterial = await _client.PutAsJsonAsync($"https://localhost:7007/api/Material/update-{ct.Id}", ct);
                return RedirectToAction("GetAllMaterial");
            }
            catch
            {
                return View();
            }
        }

        // GET: MaterialController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteMaterial = await _client.DeleteAsync($"https://localhost:7007/api/Material/delete-{id}");
            return RedirectToAction("GetAllMaterial");
        }
    }
}
