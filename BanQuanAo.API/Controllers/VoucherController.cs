using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private IVoucherService _voucherservice;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherservice = voucherService;
        }
        [HttpGet("get-vouchers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _voucherservice.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Voucher>> Get(Guid id)
        {
            try
            {
                return Ok(await _voucherservice.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("TangGiamSoLuongTheoId")]
        public async Task<ActionResult<Voucher>> TangGiamSoLuongTheoId(Guid voucherId, bool tanggiam)
        {
            try
            {
                return Ok(await _voucherservice.TangGiamSoLuongTheoId( voucherId,  tanggiam));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpGet("getbycode/{code}")]
        public async Task<ActionResult<Voucher>> Get(string code)
        {
            try
            {
                return Ok(await _voucherservice.GetItemByCode(code));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpPost("create-voucher")]
        public async Task<ActionResult<Voucher>> Post(Voucher voucher)
        {
            var result = await _voucherservice.Create(voucher);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-voucher-{id}")]
        public async Task<ActionResult<Voucher>> Put(Guid id, Voucher voucher)
        {
            var result = await _voucherservice.Update(id, voucher);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-voucher-{id}")]
        public async Task<ActionResult<Voucher>> Delete(Guid id)
        {
            var result = await _voucherservice.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
