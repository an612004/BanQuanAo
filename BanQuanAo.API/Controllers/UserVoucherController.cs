using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVoucherController : ControllerBase
    {
        private IUserVoucherService _userVoucherService;
        public UserVoucherController(IUserVoucherService userVoucherService)
        {
            _userVoucherService= userVoucherService;
        }

        [HttpGet("get-uservoucher")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _userVoucherService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<UserVoucher>> Get(Guid id)
        {
            try
            {
                return Ok(await _userVoucherService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("updatetrangthai")]
        public async Task<ActionResult<UserVoucher>> UpdateTrangThai(Guid voucherId, Guid userId, bool status)
        {
            try
            {
                return Ok(await _userVoucherService.UpdateTrangThai(voucherId,userId,status));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpGet("getsohuu/{voucherid}/{userid}")]
        public async Task<ActionResult<UserVoucher>> GetByVoucherId(Guid voucherId, Guid userId)
        {
            try
            {
                return Ok(await _userVoucherService.GetSoHuu(voucherId,userId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpPost("create-uservoucher")]
        public async Task<ActionResult<UserVoucher>> Post([FromBody] UserVoucher userVoucher)
        {
            var result = await _userVoucherService.Create(userVoucher);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpPut("update-uservoucher-{id}")]
        public async Task<ActionResult<UserVoucher>> Put(Guid id, UserVoucher userVoucher)
        {
            var result = await _userVoucherService.Update(id, userVoucher);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-uservoucher-{id}")]
        public async Task<ActionResult<UserVoucher>> Delete(Guid id)
        {
            var result = await _userVoucherService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
