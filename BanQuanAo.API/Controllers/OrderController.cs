using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IOrderService _iorderService;
        public OrderController(IOrderService orderService) {
            _iorderService = orderService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iorderService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("GetAllDonMuaChiTiet")]
        public async Task<List<DonMuaChiTietVM>> GetAllDonMuaCT(Guid idHoaDon)
        {
            var listDonMuaCT = await _iorderService.getAllDonMuaChiTiet(idHoaDon);
            return listDonMuaCT;
        }
        [HttpGet("GetAllDonMuaByOrderCode")]
        public async Task<ActionResult> GetDonMuaByOrderCode(string orderCode)
        {
           return Ok( await _iorderService.GetDonMuaByOrderCode(orderCode));
            
            
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<Order>> Get(Guid id)
        {
            try
            {
                return Ok(await _iorderService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("GetQLHDWithDetails")]
        public async Task<ActionResult<QLHDViewModel>> GetQLHDWithDetails(Guid orderId)
        {
            try
            {
                return Ok(await _iorderService.GetQLHDWithDetails(orderId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<OrderVM>> Post([FromBody] OrderVM obj)
        {
            var result = await _iorderService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
       
        [HttpPut("update-{id}")]
        public async Task<ActionResult<OrderVM>> Put(Guid id, Guid userid,[FromBody] OrderVM obj)
        {
            var result = await _iorderService.Update(id,userid, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpGet("updatestatus")]
        public async Task<ActionResult<OrderVM>> UpdateStatus(Guid OrderId, Guid StatusId)
        {
            var result = await _iorderService.UpdateStatus(OrderId, StatusId);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }

        [HttpDelete("delete-{id}")]
        public async Task<ActionResult<OrderVM>> Delete(Guid id)
        {
            var result = await _iorderService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("updatett")]
        public async Task<bool> UpdateTrangThai(Guid idhoadon, Guid trangthai, Guid? idnhanvien)
        {
            return  await _iorderService.UpdateTrangThaiGiaoHang(idhoadon, trangthai, idnhanvien);
        }
        [HttpPut("GiaoThanhCong")]
        public IActionResult GiaoThanhCong(Guid idhd, Guid idnv)
        {
            var result = _iorderService.ThanhCong(idhd, idnv);
            return Ok(result);
        }
        [HttpPut("HuyHD")]
        public async Task<IActionResult> HuyHD(Guid idhd, Guid idnv)
        {
            var result = await _iorderService.HuyHD(idhd, idnv);
            return Ok(result);
        }
        [HttpPut("UpdateGhichu")]
        public bool UpdateGhiChuHD(Guid idhd, Guid idnv, string ghichu)
        {
            return _iorderService.UpdateGhiChuHD(idhd, idnv, ghichu);
        }
        [HttpGet("GetAllDonMuaChiTiet1/{idhd}")]
        public async Task<IActionResult> GetAllDonMuaCT1(Guid idhd)
        {
            var listDonMuaCT = await _iorderService.getAllDonMuaChiTiet1(idhd);
            return Ok(listDonMuaCT);
        }
    }
}
