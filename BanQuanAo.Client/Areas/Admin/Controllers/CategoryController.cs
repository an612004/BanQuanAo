using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly HttpClient _client;
        public CategoryController(HttpClient client)
        {
            _client = client;   
        }
        // GET: CategoryController
        public async Task<IActionResult> GetAllCategory()
        {
            var allCategory = await _client.GetFromJsonAsync<List<Category>>("https://localhost:7007/api/Category/get-all-Category");
            return View(allCategory);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var getCategory = await _client.GetFromJsonAsync<Category>($"https://localhost:7007/api/Category/get-{id}");
            return View(getCategory);
        }

        // GET: CategoryController/Create
        public async Task<IActionResult> Create()
        {
           
            var responseLoaiSP = _client.GetAsync("https://localhost:7007/api/Category/get-all-Category").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<Category>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        
        public async Task<IActionResult> Create(Category ct)
        {
            try
            {
                ct.Id = Guid.NewGuid();
                string apiURL = $"https://localhost:7007/api/Category/create-Category";
                var content = new StringContent(JsonConvert.SerializeObject(ct), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiURL, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllCategory");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var getCategory = await _client.GetFromJsonAsync<Category>($"https://localhost:7007/api/Category/get-{id}");
            return View(getCategory);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category ct)
        {
            try
            {
                var editCategory = await _client.PutAsJsonAsync($"https://localhost:7007/api/Category/update-Category-{ct.Id}", ct);
                return RedirectToAction("GetAllCategory");
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteCategory = await _client.DeleteAsync($"https://localhost:7007/api/Category/delete-Category-{id}");
            return RedirectToAction("GetAllCategory");
        }
    }
}
