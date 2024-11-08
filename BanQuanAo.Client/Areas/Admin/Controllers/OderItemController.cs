using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{

    public class OderItemController : Controller
    {
        HttpClient _client;

        public OderItemController(HttpClient httpClient)
        {
            _client = httpClient;
        }
        [HttpGet]
        public async Task<IActionResult> ShowList()
        {
            string apiurl = "https://localhost:7007/api/orderitem/getall";
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<OrderItem>>(data);
            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> DetailsOrder(Guid id)
        {

            string apiurl = $"https://localhost:7007/api/orderitem/details/{id}";
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderItem>(data);
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {


            string apiurl = "https://localhost:7007/api/orderitem/add";
            var data = JsonConvert.SerializeObject(order);
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

            string apiurl = $"https://localhost:7007/api/orderitem/details/{id}";
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderItem>(data);
            return View(result);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderItem order)
        {

            string apiurl = $"https://localhost:7007/api/orderitem/update/{order.Id} ";
            var data = JsonConvert.SerializeObject(order);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(apiurl, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }

        public async Task<IActionResult> DeleteC(Guid id)
        {

            string apiurl = $"https://localhost:7007/api/orderitem/delete/{id}";
            var response = await _client.DeleteAsync(apiurl);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }
    }
}
