using BanQuanAo.Client.Models;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> IndexAsync()
        {
            // show role trong index 
            // nếu là tài khoản của nhân viên thì được phép reset mật khẩu
            // Lấy danh sách vai trò từ API
            var roles = await _httpClient.GetFromJsonAsync<List<Role>>("https://localhost:7007/api/role");
            var roleNames = roles.Select(role => role.Name).ToList();
            ViewData["RoleList"] = roleNames;
            var user =await _httpClient.GetFromJsonAsync<List<UserViewModel>>("https://localhost:7007/api/users/getall");
            return View(user);
        }
        public async Task<IActionResult> DeleteUser(string userName)
        {
            await _httpClient.DeleteAsync($"https://localhost:7007/api/users/delete/{userName}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangeRole(string userName, string roleName)
        {

            // Lấy thông tin người dùng và vai trò hiện tại
            var response = await _httpClient.GetAsync($"https://localhost:7007/api/users/change-role/{userName}/{roleName}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Nếu không thành công, trả về thông báo lỗi
                return BadRequest();
            };
        }


    }
}
