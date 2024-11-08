
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RateController : Controller
    {
        HttpClient _client;

        public RateController(HttpClient httpClient)
        {
            _client = httpClient;
        }
        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            string apiurl = "https://localhost:7007/api/rate/getall";
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Rate>>(data);
            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {

            string apiurl = "https://localhost:7007/api/rate/details/";
            var response = await _client.GetAsync(apiurl + id);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Rate>(data);
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Rate rate)
        {

           
            string apiurl = "https://localhost:7007/api/rate/add";
            var data = JsonConvert.SerializeObject(rate);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(apiurl, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            string apiurl = "https://localhost:7007/api/rate/";
            var response = await _client.GetAsync(apiurl + id);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Rate>(data);
            return View(result);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(Rate rate)
        {

            string apiurl = "https://localhost:7007/api/rate/update/";
            var data = JsonConvert.SerializeObject(rate);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(apiurl + rate.Id, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }

        
        public async Task<IActionResult> DeleteC(Guid id, Guid orderid)
        {
            
            string apiurl = $"https://localhost:7007/api/rate/delete?id={id}&orderid={orderid}";

            var response = await _client.DeleteAsync(apiurl);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }
    }
}
