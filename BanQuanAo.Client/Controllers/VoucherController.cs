using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace BanQuanAo.Client.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;

        public VoucherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<IActionResult> GetVoucherForU()
        //{
        //    try
        //    {
        //        // Lấy thông tin người dùng đang đăng nhập từ HttpContext
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        ViewBag.UserId = userId;
        //        if (!string.IsNullOrEmpty(userId))
        //        {
        //            var allvoucherU = await _httpClient.GetFromJsonAsync<List<UserVoucher>>("https://localhost:7007/api/UserVoucher/get-uservoucher");
        //            // Lọc danh sách UserVoucher của người dùng đăng nhập
        //            //var userVouchers = allvoucherU.Where(uv => uv.UserId == Guid.Parse(userId)).ToList();
        //            // Lấy danh sách voucherIds từ userVouchers
        //            //var voucherIds = userVouchers.Select(uv => uv.VoucherId).ToList();

        //            // Lấy danh sách Voucher tương ứng
        //            var allVouchers = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");

        //            //var userVouchersWithVouchers = allVouchers.Where(v => voucherIds.Contains(v.Id)).ToList();
        //            //ViewData["UVC"] = userVouchersWithVouchers;
        //            var lstvc = from a in allvoucherU
        //                        join b in allVouchers on a.VoucherId equals b.Id
        //                        where a.UserId == Guid.Parse(userId) // Lọc theo UserId
        //                        select new
        //                        {
        //                            UserVoucher = a,
        //                            Voucher = b
        //                        };
        //            ViewData["Lstvc"] = lstvc;
        //            return View();
        //        }
        //        else
        //        {
        //            // Người dùng chưa đăng nhập, xử lý theo yêu cầu của bạn
        //            return RedirectToAction("Login");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi nếu cần
        //        Console.WriteLine(ex.Message);
        //        return View(); 
        //    }
        //}
        public class UserVoucherViewModel
        {
            public UserVoucher UserVoucher { get; set; }
            public Voucher Voucher { get; set; }
        }
        public async Task<IActionResult> GetVoucherForU(int? page/*,int status*/)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            //ViewBag.status = status;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var allvoucherU = await _httpClient.GetFromJsonAsync<List<UserVoucher>>("https://localhost:7007/api/UserVoucher/get-uservoucher");
                    
                var allVouchers = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");
                var lstvc = from a in allvoucherU
                            join b in allVouchers on a.VoucherId equals b.Id
                            where a.UserId == Guid.Parse(userId) // Lọc theo UserId
                            select new UserVoucherViewModel
                            {
                                UserVoucher = a,
                                Voucher = b
                            };
                ViewData["Lstvc"] = lstvc;
                //if (status == 0)
                //{
                //    lstvc = lstvc;
                //}
                //if (status == 1)
                //{
                //    lstvc = lstvc.Where(c => c.UserVoucher.Status == true);
                //}
                //if (status == 2)
                //{
                //    lstvc = lstvc.Where(c=>c.UserVoucher.Status == false);
                //}

                return View(lstvc.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }
    }
}
