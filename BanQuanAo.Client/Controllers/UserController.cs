using AspNetCoreHero.ToastNotification.Abstractions;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;

namespace BanQuanAo.Client.Controllers
{
    public class UserController : Controller
    {
        public INotyfService _notyf;

        private readonly HttpClient _httpClient;

        public UserController(HttpClient httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        [HttpGet]

        public async Task<IActionResult> Profile(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;



            var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            ViewBag.userID = userID;
            var user = await _httpClient.GetFromJsonAsync<User>($"https://localhost:7007/api/users/getID/{id}");
            var ranks = await _httpClient.GetFromJsonAsync<List<Rank>>("https://localhost:7007/api/ranks/get-ranks");

            ViewData["rank"] = ranks;
            return View(user);

        }

        [HttpPost]

        public async Task<IActionResult> Profile(UserViewModel user)
        {

            if (user.PhoneNumber.Length > 11)
            {
                _notyf.Error("Số điện thoại không quá 10 số");
            }
            if (!IsValidEmail(user.Email))
            {
                _notyf.Error("Email không hợp lệ");

            }
            if (user.DateOfBirth > DateTime.Now || user.DateOfBirth < new DateTime(1900, 1, 1))
            {
                _notyf.Error("ngày sinh không hợp lệ, ngày sinh phải nhỏ hơn hiện tại và ko được thấp hơn năm 1900");

            }
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var usernameClaim = identity.FindFirst(ClaimTypes.Name);

            if (usernameClaim != null)
            {
                var username = usernameClaim.Value;
                user.UserName = username;
                var result = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/users/Update", user);
                if (result.IsSuccessStatusCode)
                {
                    _notyf.Success("Cập nhật thành công");
                    return RedirectToAction("Index", "Home");

                }
                // Sử dụng biến username ở đây
            }


            _notyf.Error("Cập nhật thất bại");
            return RedirectToAction("Profile");

        }


    }
    
}
