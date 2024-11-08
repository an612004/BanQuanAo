using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using X.PagedList;

namespace BanQuanAo.Client.Controllers
{
    public class PostController : Controller
    {
        private HttpClient _httpClient;

        public PostController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> ListPostAsync(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var Post = await _httpClient.GetFromJsonAsync<List<Post>>("https://localhost:7007/api/posts/get-posts");
            return View(Post.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> DetailPost(Guid id)
        {
            var post = await _httpClient.GetFromJsonAsync<Post>($"https://localhost:7007/api/posts/get/{id}");
            var lstUser = await _httpClient.GetFromJsonAsync<List<User>>($"https://localhost:7007/api/users/getall");
            ViewBag.user = lstUser;
            return View(post);
        }
    }
}
