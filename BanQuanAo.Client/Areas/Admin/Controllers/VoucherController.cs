using BanQuanAo.Share.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using System.Net.Http.Json;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {


        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public VoucherController(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;

            ScheduleUpdateVoucherStatus();
        }
        // Hàm thực hiện cập nhật trạng thái voucher
        public async Task UpdateVoucherStatus()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var allVouchers = await httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");

            foreach (var voucher in allVouchers)
            {
                //kiểm tra ngày kết thúc nếu < hiện tại thì tự đổi trạng thái
                if (voucher.End_Date < DateTime.Now && voucher.Status == true)
                {
                    voucher.Status = false;
                    await httpClient.PutAsJsonAsync($"https://localhost:7007/api/voucher/update-voucher-{voucher.Id}", voucher);
                }
            }
        }

        // Hàm định kỳ để cập nhật trạng thái voucher

        public void ScheduleUpdateVoucherStatus()
        {
            var interval = TimeSpan.FromMinutes(1); // Cập nhật mỗi phút
            var timer = new Timer(async _ => await UpdateVoucherStatus(), null, TimeSpan.Zero, interval);
        }
        public async Task<IActionResult> GetallVoucher()
        {

            var allvoucher = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");

            return View(allvoucher);
        }

        //addvoucher rank
        public async Task<IActionResult> AddVoucherForRank(string selectedRank, Voucher vc)
        {
            if (vc.MaxDiscountAmount == null)
            {
                vc.MaxDiscountAmount = vc.Value;
            }
            // Tạo voucher mới
            Voucher voucher = new Voucher
            {
                Id = Guid.NewGuid(),
                Code = vc.Code,
                Quantity = vc.Quantity,
                Value = vc.Value,
                Description = vc.Description,
                MaxDiscountAmount = vc.MaxDiscountAmount,
                Discount_Type = vc.Discount_Type,
                Minimum_order_value = vc.Minimum_order_value,
                Create_Date = DateTime.Now,
                Start_Date = vc.Start_Date,
                End_Date = vc.End_Date,
                Status = true
            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/voucher/create-voucher", voucher);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Unable to create voucher.");
            }

            // Lấy danh sách tất cả rank
            var allranks = await _httpClient.GetFromJsonAsync<List<Rank>>("https://localhost:7007/api/ranks/get-ranks");
            // lấy danh sách tất cả người dùng
            var allUser = await _httpClient.GetFromJsonAsync<List<User>>("https://localhost:7007/api/users/getall");
            var usersInRank = allUser;
            if (selectedRank != "tatca")
            {
                var rank = allranks.FirstOrDefault(r => r.Name == selectedRank);
                if (rank == null)
                {
                    return BadRequest("không có");
                }
                // Lấy danh sách người dùng theo rank đã chọn
                usersInRank = allUser.Where(c => c.RankId == rank.Id).ToList();
            }

            // Gán voucher cho mỗi người dùng trong rank
            foreach (var user in usersInRank)
            {
                UserVoucher uv = new UserVoucher();
                uv.Id = Guid.NewGuid();
                uv.VoucherId = voucher.Id;
                uv.UserId = user.Id;
                uv.Status = true;
                // Tạo UserVoucher 
                var createUV = await _httpClient.PostAsJsonAsync("https://localhost:7007/api/UserVoucher/create-uservoucher", uv);
                if (!createUV.IsSuccessStatusCode)
                {
                    return BadRequest();
                }


            }

            TempData["SuccessMessage"] = "Voucher đã được tạo thành công!";
            var successMessage = TempData["SuccessMessage"] as string;
            if (!string.IsNullOrEmpty(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
                ViewData["ShowSuccessMessage"] = true;
            }
            return RedirectToAction("GetallVoucher");
        }

        public async Task<IActionResult> CreateVoucher()
        {
            var allranks = await _httpClient.GetFromJsonAsync<List<Rank>>("https://localhost:7007/api/ranks/get-ranks");
            ViewData["lstRank"] = allranks;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(string selectedRank, Voucher voucher)
        {
            try
            {
                var allranks = await _httpClient.GetFromJsonAsync<List<Rank>>("https://localhost:7007/api/ranks/get-ranks");
                ViewData["lstRank"] = allranks;
                var allvoucher = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");
                if (voucher.Minimum_order_value != null || voucher.Code != null || voucher.Value != null || voucher.Discount_Type != null || voucher.Start_Date != null || voucher.End_Date != null)
                {
                    if (voucher.Minimum_order_value < 0)
                    {
                        ViewData["SoTienCan"] = "Số tiền cần không được âm ";
                    }
                    if (voucher.Value <= 0)
                    {
                        ViewData["GiaTri"] = "Mời bạn nhập giá trị lớn hơn 0";
                    }
                    if (voucher.Quantity <= 0)
                    {
                        ViewData["SoLuong"] = "Mời bạn nhập số lượng lớn hơn 0";
                    }
                    if (voucher.End_Date < voucher.Start_Date)
                    {
                        ViewData["NgayKetThuc"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";
                    }
                    if (voucher.Start_Date <= DateTime.Now)
                    {
                        ViewData["NgayBd"] = "Ngày bắt đầu phải lớn hơn ngày hiện tại";
                        return View(voucher);
                    }
                    var timkiem = allvoucher.FirstOrDefault(x => x.Code == voucher.Code.Trim());
                    if (timkiem != null)
                    {
                        ViewData["Ma"] = "Mã này đã tồn tại";
                    }

                    if (voucher.Discount_Type == 1)
                    {
                        if (voucher.Minimum_order_value == 0)
                        {
                            if (voucher.Value > 100 || voucher.Value <= 0)
                            {
                                ViewData["GiaTri"] = "Giá trị từ 1 đến 100";
                                return View(voucher);
                            }
                            if (voucher.Value <= 100 && voucher.Value > 0)
                            {
                                if (voucher.Minimum_order_value >= 0 && voucher.Value > 0 && voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date && timkiem == null)
                                {
                                    await AddVoucherForRank(selectedRank, voucher);
                                }
                            }
                        }
                        if (voucher.Minimum_order_value > 0)
                        {
                            if (voucher.Value <= voucher.Minimum_order_value)
                            {
                                if (voucher.Value <= 100 && voucher.Value > 0)
                                {
                                    if (voucher.Minimum_order_value >= 0 && voucher.Value > 0 && voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date && timkiem == null)
                                    {
                                        await AddVoucherForRank(selectedRank, voucher);
                                    }
                                }
                                if (voucher.Value > 100 || voucher.Value <= 0)
                                {
                                    ViewData["GiaTri"] = "Giá trị từ 1 đến 100";
                                    return View(voucher);
                                }

                            }
                            if (voucher.Value > voucher.Minimum_order_value)
                            {
                                ViewData["GiaTri"] = "Giá trị phải nhỏ hơn hoặc bằng số tiền cần";
                                return View(voucher);

                            }
                        }


                    }
                    if (voucher.Discount_Type == 0)
                    {
                        if (voucher.Minimum_order_value == 0)
                        {
                            if (voucher.Value <= 0)
                            {
                                ViewData["GiaTri"] = "Giá trị phải lớn hơn 0";
                                return View(voucher);
                            }
                            if (voucher.Value > 0)
                            {
                                if (voucher.Minimum_order_value >= 0 && voucher.Value > 0 && voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date && timkiem == null)
                                {
                                    await AddVoucherForRank(selectedRank, voucher);
                                }
                            }
                        }
                        if (voucher.Minimum_order_value > 0)
                        {
                            if (voucher.Value <= voucher.Minimum_order_value)
                            {
                                if (voucher.Value <= 0)
                                {
                                    ViewData["GiaTri"] = "Giá trị phải lớn hơn 0";
                                    return View(voucher);
                                }
                                if (voucher.Value > 0)
                                {
                                    if (voucher.Minimum_order_value >= 0 && voucher.Value > 0 && voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date && timkiem == null)
                                    {
                                        await AddVoucherForRank(selectedRank, voucher);
                                    }
                                }
                            }
                            if (voucher.Value > voucher.Minimum_order_value)
                            {
                                ViewData["GiaTri"] = "Giá trị phải nhỏ hơn hoặc bằng số tiền cần";
                                return View(voucher);
                            }
                        }
                    }

                }
                return View();
            }
            catch
            {
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> editVoucher(Guid id)
        {

            var result = await _httpClient.GetFromJsonAsync<Voucher>($"https://localhost:7007/api/voucher/getbyid/{id}");

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> editVoucher(Voucher voucher)
        {
            try
            {
                if (voucher.Start_Date <= DateTime.Now)
                {
                    ViewData["NgayBd"] = "Ngày bắt đầu phải lớn hơn ngày hiện tại";
                }
                if (voucher.End_Date < voucher.Start_Date)
                {
                    ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";
                }
                if (voucher.Quantity <= 0)
                {
                    ViewData["SoLuong"] = "Mời bạn nhập số lượng lớn hơn 0";
                }

                if (voucher.Minimum_order_value == 0)
                {

                    if (voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date)
                    {


                        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/voucher/update-voucher-{voucher.Id}", voucher);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAllVoucher");
                        }
                        return View();
                    }
                }
                if (voucher.Minimum_order_value > 0)
                {
                    if (voucher.Minimum_order_value < voucher.Value)
                    {
                        ViewData["SoTienCan"] = "Số tiền cần phải lớn hơn giá trị";

                    }
                    if (voucher.Minimum_order_value >= voucher.Value)
                    {
                        if (voucher.Quantity > 0 && voucher.End_Date >= voucher.Start_Date)
                        {

                            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/voucher/update-voucher-{voucher.Id}", voucher);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("GetAllVoucher");
                            }
                            return View();
                        }
                    }

                }
                else
                {
                    if (voucher.Minimum_order_value < 0)
                    {
                        ViewData["SoTienCan"] = "Mời bạn nhập số tiền cần không âm";
                    }

                    return View();
                }
                return View();
            }
            catch
            {
                return View();
            }

        }

        public async Task<IActionResult> DeleteVoucher(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7007/api/voucher/delete-voucher-{id}");
            return RedirectToAction("GetallVoucher");
        }
        public async Task<IActionResult> SuDung(Guid id)
        {
            try
            {
                var allvoucher = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");
                var timkiem = allvoucher.FirstOrDefault(x => x.Id == id);
                if (timkiem != null)
                {
                    timkiem.Status = true;
                    var response = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/voucher/update-voucher-{timkiem.Id}", timkiem);
                    return RedirectToAction("GetAllVoucher");
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
        public async Task<IActionResult> KoSuDung(Guid id)
        {
            try
            {
                var allvoucher = await _httpClient.GetFromJsonAsync<List<Voucher>>("https://localhost:7007/api/voucher/get-vouchers");
                var timkiem = allvoucher.FirstOrDefault(x => x.Id == id);
                if (timkiem != null)
                {
                    timkiem.Status = false;
                    var response = await _httpClient.PutAsJsonAsync($"https://localhost:7007/api/voucher/update-voucher-{timkiem.Id}", timkiem);
                    return RedirectToAction("GetAllVoucher");
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
    }


}
