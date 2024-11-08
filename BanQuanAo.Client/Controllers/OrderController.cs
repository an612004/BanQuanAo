using AspNetCoreHero.ToastNotification.Abstractions;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;
using X.PagedList;

namespace BanQuanAo.Client.Controllers
{
	public class OrderController : Controller
	{
		private readonly HttpClient _httpClient;
        public INotyfService _notyf;

        public OrderController(HttpClient httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }

        public async Task<IActionResult> allOrder(Guid id, int? page,int status)
        {

            if (page == null) page = 1;
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.status = status;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = identity.FindFirst(ClaimTypes.Name);
            var userName = userIdClaim.Value;
            var getUserbyName = await _httpClient.GetFromJsonAsync<User>($"https://localhost:7007/api/users/get/{userName}");
            var listOrder = await _httpClient.GetFromJsonAsync<List<Order>>("https://localhost:7007/api/order/getall");
            var listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id).OrderByDescending(c=>c.Create_Date);
            
            if (status==-1)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("9C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);

            }
            if (status==0)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id).OrderByDescending(c => c.Create_Date);
            }
            if (status==1)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("1C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);
            }
            if (status==2)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("2C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);
            }
            if (status==3)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("3C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);
            }
            if (status==4)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);
            }
            if (status==5)
            {
                listorderbyUser = listOrder.Where(c => c.UserId == getUserbyName.Id&& c.OrderStatusId == Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).OrderByDescending(c => c.Create_Date);
            }

            return View(listorderbyUser.ToPagedList(pageNumber, pageSize));

        }
        public IActionResult OrderDetails(Guid idHoaDon)
        {
            
                List<DonMuaChiTietVM> DonMuaCT = new List<DonMuaChiTietVM>();
                HttpResponseMessage responseDonMuaCT =  _httpClient.GetAsync($"https://localhost:7007/api/order/GetAllDonMuaChiTiet?idHoaDon={idHoaDon}").Result;
                if (responseDonMuaCT.IsSuccessStatusCode)
                {
                    DonMuaCT = JsonConvert.DeserializeObject<List<DonMuaChiTietVM>>(responseDonMuaCT.Content.ReadAsStringAsync().Result);
                }
                return View("OrderDetails", DonMuaCT);
            
        }
        public async Task<IActionResult> HuyDon(Guid idHoaDon, Guid? idTrangThai)
        {
            var listOrder = await _httpClient.GetFromJsonAsync<List<Order>>("https://localhost:7007/api/order/getall");
            
            var firstOrder = listOrder.FirstOrDefault();
            if (firstOrder != null)
            {
                idTrangThai = firstOrder.OrderStatusId = Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");//huỷ đơn
            }
            //update trạng thái hoá đơn
            HttpResponseMessage responseDonMua = await _httpClient.GetAsync($"https://localhost:7007/api/order/updatestatus?OrderId={idHoaDon}&StatusId={idTrangThai}");
            
            if (responseDonMua.IsSuccessStatusCode)
            {
                //hoàn lại voucher
                if (firstOrder.VoucherId != null)
                {
                    // + số lượng voucher 

                    var updateSL = await _httpClient.GetAsync($"https://localhost:7007/api/voucher/TangGiamSoLuongTheoId?voucherId={firstOrder.VoucherId}&tanggiam=true");

                    //  sửa trạng thái trong uservoucher  

                    var updateStatus = await _httpClient.GetAsync($" https://localhost:7007/api/UserVoucher/updatetrangthai?voucherId={firstOrder.VoucherId}&userId={firstOrder.UserId}&status=true");

                }
                //hoàn lại số lượng
                //var listOrderItem = await _httpClient.GetFromJsonAsync<List<OrderItem>>("https://localhost:7007/api/orderitem/getall");           
                
                //var lsthdct = listOrderItem.Where(c => c.OrderId == idHoaDon).ToList();
                //foreach (var hdct in lsthdct)
                //{
                //    var UpdateQ = await _httpClient.GetAsync($"https://localhost:7007/api/productDetail/UpdateQuantityOrderFail?productDetailId={hdct.ProductDetailId}&quantity={hdct.Quantity}");// update lại sp
                //}
                //var updateSLSPfromDb = await _httpClient.GetAsync($"https://localhost:7007/api/product/UpdateSLTheoSPCT");
                return RedirectToAction("allOrder");
            }

            return RedirectToAction("allOrder");
        }

        public async Task<IActionResult> allOrderCode(string ordercode)
        {
            if (string.IsNullOrEmpty(ordercode))
            {
                _notyf.Warning("Mã đơn hàng không được để trống.");
                return RedirectToAction("SearchOrderCode");
            }
            var listOrder = await _httpClient.GetFromJsonAsync<QLHDViewModel>($"https://localhost:7007/api/order/GetAllDonMuaByOrderCode?orderCode={ordercode}");

            return View(listOrder);
        }

        public async Task<IActionResult> SearchOrderCode()
        {

            return View();

        }



    }
}
