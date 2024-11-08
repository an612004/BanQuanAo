using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using X.PagedList;

namespace BanQuanAo.Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        HttpClient _client;

        public OrderController(HttpClient httpClient)
        {
            _client = httpClient;
        }
        [HttpGet]
        public async Task<IActionResult> ShowList(int? page,int? status, string searchText, DateTime? startDate, DateTime? endDate)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.status = status;
            ViewBag.searchText = searchText;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            var user = await _client.GetFromJsonAsync<List<UserViewModel>>("https://localhost:7007/api/users/getall");
            ViewData["User"] = user;
            string apiurl = "https://localhost:7007/api/order/getall";
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Order>>(data);
            var Lst = await _client.GetFromJsonAsync<List<Order>>(apiurl);
            var LstOrder = Lst.Where(c => c.Id !=null);
            if (status==1)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("1C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==2)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("2C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==3)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("3C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==4)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==5)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("5C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==6)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==7)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("7C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (status==8)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("8C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
             if (status==9)
            {
                LstOrder = LstOrder.Where(c => c.OrderStatusId == Guid.Parse("9C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"));
            }
            if (searchText!=null)
            {
                LstOrder = LstOrder.Where(c => c.OrderCode.ToLower().Contains(searchText.ToLower()));
            }
            if (startDate != null && endDate != null)
            {
                LstOrder = LstOrder.Where(c => c.Create_Date >= startDate && c.Create_Date <= endDate);
            }
            int totalItems = LstOrder.Count(); // Điều này có thể thay đổi tùy theo cách bạn lấy dữ liệu
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Kiểm tra nếu trang hiện tại lớn hơn tổng số trang, chuyển về trang 1
            if (totalPages>0)
            {
                if (page > totalPages)
                {
                    return RedirectToAction("ShowList", new { page = 1, status,  searchText,  startDate,  endDate });
                }
            }
           
            return View(LstOrder.ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public async Task<IActionResult> DetailsOrder(Guid id)
        {

            string apiurl = $"https://localhost:7007/api/order/details/{id}" ;
            var response = await _client.GetAsync(apiurl);
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Order>(data);
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {

           
            string apiurl = "https://localhost:7007/api/order/add";
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

            string apiurl = $"https://localhost:7007/api/order/details/{id}";
            var response = await _client.GetAsync(apiurl );
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Order>(data);
            return View(result);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {

            string apiurl = $"https://localhost:7007/api/order/update/{order.Id} ";
            var data = JsonConvert.SerializeObject(order);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(apiurl , stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }

        public async Task<IActionResult> DeleteC(Guid id )
        {

            string apiurl = $"https://localhost:7007/api/order/delete/{id}";

            var response = await _client.DeleteAsync(apiurl);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowList");
            }
            return View();
        }
        public async Task<IActionResult> DoiTrangThai(Guid idhd, int trangthai)// Dùng cho trạng thái truyền  vào: 10, 3
        {

            try
            {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

                
                if (identity != null)
                {
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value); // userId


                    var idnv = userID;
                   
                     if(trangthai == 2) // chờ lấy hàng
                    {
                        var hoadonCT =await _client.GetFromJsonAsync<DonMuaChiTietVM>($"https://localhost:7007/api/order/GetAllDonMuaChiTiet1/{idhd}");
                        foreach (var item in hoadonCT.OrderItem)
                        {
                            var responseUpdateQuantityProductDetail = await _client.GetAsync($"https://localhost:7007/api/productDetail/UpdateQuantityById?productDetailId={item.ProductDetailId}&quantity={item.Quantity}");// update lại sp
                            if (!responseUpdateQuantityProductDetail.IsSuccessStatusCode)
                            {// xác nhận đơn xong thì mới trừ số lượng sp
                                return BadRequest();
                            }
                        }
                        var updateSLSPfromDb = await _client.GetAsync($"https://localhost:7007/api/product/UpdateSLTheoSPCT");
                        if (!updateSLSPfromDb.IsSuccessStatusCode)
                        { // update lại slsp
                            return BadRequest();

                        }
                        Guid idtt = Guid.Parse("2C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");
                        string url = $"https://localhost:7007/api/order/updatett?idhoadon={idhd}&trangthai={idtt}&idnhanvien={idnv}";
                        var response = await _client.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {


                            TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
                            var successMessage = TempData["SuccessMessage"] as string;
                            if (!string.IsNullOrEmpty(successMessage))
                            {
                                ViewData["SuccessMessage"] = successMessage;
                                ViewData["ShowSuccessMessage"] = true;
                            }
                            return RedirectToAction("ShowList");

                        }
                    }
                    else if (trangthai == 3)// dang giao hàng
                    {
                        Guid idtt = Guid.Parse("3C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");
                        string url = $"https://localhost:7007/api/order/updatett?idhoadon={idhd}&trangthai={idtt}&idnhanvien={idnv}";
                        var response = await _client.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.SuccessMessage = "Cập nhật trạng thái thành công";
                            return RedirectToAction("ShowList");

                        }
                    }
                    else if (trangthai == 4)//giao hàng thành công
                    {
                        string url = $"https://localhost:7007/api/order/GiaoThanhCong?idhd={idhd}&idnv={idnv}";
                        var response = await _client.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.SuccessMessage = "Cập nhật trạng thái thành công";
                            return RedirectToAction("ShowList");

                        }
                    }
                    else if (trangthai == 5)//giao hàng không thành công
                    {
                        Guid idtt = Guid.Parse("5C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");
                        string url = $"https://localhost:7007/api/order/updatett?idhoadon={idhd}&trangthai={idtt}&idnhanvien={idnv}";
                        var response = await _client.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.SuccessMessage = "Cập nhật trạng thái thành công";
                            return RedirectToAction("ShowList");

                        }
                    }
                   
                }
                ViewBag.ErrorMessage = "Cập nhật trạng thái thất bại";
                return RedirectToAction("ShowList");

            }
            catch (Exception)
            {
                return RedirectToAction("ShowList", "Order");
            }
        }
        [HttpGet("/Order/HuyHD")]
        public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        {
            try
            {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    var userID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value); // userId


                    var idnv = userID;
                    if (ghichu != null)
                    {
                        string url = $"https://localhost:7007/api/order/HuyHD?idhd={idhd}&idnv={idnv}";
                        var response = await _client.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            var stringURL = $"https://localhost:7007/api/order/UpdateGhichu?idhd={idhd}&idnv={idnv}&ghichu={ghichu}";
                            var responseghichu = await _client.PutAsync(stringURL, null);
                            if (responseghichu.IsSuccessStatusCode)
                            {
                                ViewBag.SuccessMessage = "Cập nhật trạng thái thành công";
                                return RedirectToAction("ShowList");


                            }
                        }
                    }
                    ViewBag.ErrorMessage = "Ghi chú không được trống";

                }
                return RedirectToAction("ShowList");



            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowList", "Order");

            }
        }
        [HttpGet("/Order/ViewChiTietHD/{idhd}")]
        public async Task<IActionResult> ViewChiTietHD(string idhd)
        {
            var hd = await _client.GetFromJsonAsync<DonMuaChiTietVM>($"https://localhost:7007/api/order/GetAllDonMuaChiTiet1/{idhd}");
            if (hd == null)
            {
                return NotFound();
            }
            return PartialView("ChiTietHD", hd);
        }
    }
}
