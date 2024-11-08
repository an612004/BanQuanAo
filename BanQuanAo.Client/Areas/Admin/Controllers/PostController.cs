using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private HttpClient _httpClient;

        public PostController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<List<PostVM>>($"https://localhost:7007/api/posts/get-posts");
            return View(posts);
        }
        public IActionResult CreatePost()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post, IFormFile file, string edit)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    // Gửi dữ liệu và tệp đến API
                    string apiurl = "https://localhost:7007/api/posts/create-post";
                    // Tạo nội dung yêu cầu gửi đến API
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(Guid.NewGuid().ToString()), "Id");
                    content.Add(new StringContent(userID.ToString()), "UserId");
                    content.Add(new StringContent(post.Tittle ?? "DefaultTittle"), "Tittle");
                    content.Add(new StringContent(post.TittleImage ?? "DefaultTittleImage"), "TittleImage");
                    content.Add(new StringContent(edit ?? "DefaultContents"), "Contents");
                    content.Add(new StringContent(DateTime.Now.ToString() ?? ""), "CreateAt");
                    content.Add(new StringContent(DateTime.Now.ToString() ?? ""), "UpdateAt");
                    content.Add(new StringContent(post.Status?.ToString() ?? "0"), "Status");
                    content.Add(new StringContent(post.Description?.ToString() ?? ""), "Description");

                    // Kiểm tra và thêm file vào nội dung yêu cầu nếu có
                    if (file != null && file.Length > 0)
                    {
                        content.Add(new StreamContent(file.OpenReadStream()), "filecollection", file.FileName);
                    }

                    var response = await _httpClient.PostAsync(apiurl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Xử lý khi yêu cầu thành công
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Xử lý khi yêu cầu không thành công
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        // Trả về thông báo lỗi hoặc xử lý theo nhu cầu của bạn
                        return BadRequest(errorMessage);
                    }
                }
                // thông báo đăng nhập
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }

        public async Task<IActionResult> EditPostAsync(Guid id)
        {
            var post = await _httpClient.GetFromJsonAsync<Post>($"https://localhost:7007/api/posts/get/{id}");
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> EditPostAsync(Post post, string edit, IFormFile file)
        {
            // Gửi dữ liệu và tệp đến API
            string apiurl = "https://localhost:7007/api/posts/update-post/";
            // Tạo nội dung yêu cầu gửi đến API
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(post.Id.ToString()), "Id");
            content.Add(new StringContent(post.UserId.ToString()), "UserId");
            content.Add(new StringContent(post.Tittle ?? "DefaultTittle"), "Tittle");
            content.Add(new StringContent(post.TittleImage ?? "DefaultTittleImage"), "TittleImage");
            content.Add(new StringContent(edit ?? "DefaultContents"), "Contents");
            content.Add(new StringContent(post.CreateAt.ToString() ?? ""), "CreateAt");
            content.Add(new StringContent(DateTime.Now.ToString() ?? ""), "UpdateAt");
            content.Add(new StringContent(post.Status?.ToString() ?? "0"), "Status");
            content.Add(new StringContent(post.Description?.ToString() ?? ""), "Description");

            // Kiểm tra và thêm file vào nội dung yêu cầu nếu có
            if (file != null && file.Length > 0)
            {
                content.Add(new StreamContent(file.OpenReadStream()), "filecollection", file.FileName);
            }

            var response = await _httpClient.PutAsync(apiurl, content);
            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi yêu cầu thành công
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý khi yêu cầu không thành công
                var errorMessage = await response.Content.ReadAsStringAsync();
                // Trả về thông báo lỗi hoặc xử lý theo nhu cầu của bạn
                return BadRequest(errorMessage);
            }


        }

        [HttpGet]
        public async Task<IActionResult> DetailPost(Guid id)
        {
            var post = await _httpClient.GetFromJsonAsync<Post>($"https://localhost:7007/api/posts/get/{id}");
            var lstUser = await _httpClient.GetFromJsonAsync<List<User>>($"https://localhost:7007/api/users/getall");
            ViewBag.user = lstUser;
            return View(post);
        }

        public async Task<IActionResult> ChangeStatus(Guid id,int status)
        {
            try
            {
                var url = $"https://localhost:7007/api/posts/ChangeStatus?id={id}&status={status}";
                var resul = await _httpClient.GetAsync(url);
                if (resul.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }

                return BadRequest("cant get");
            }
            catch
            {
                return BadRequest("Bug");
            }

        }
    }
}
