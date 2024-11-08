using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
    public class AdressController : ControllerBase
    {
        private IAdressService _adressService;
        public AdressController(IAdressService iadressService)
        {
            _adressService = iadressService;
        }
        // GET: api/<AdressController>
        /// <summary>
        /// Get All Address
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get-All-Adress")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _adressService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500,"Không tìm được dữ liệu");
            }
        }

        // GET api/<AdressController>/5
        [HttpGet("Get-Adress-ById")]
        public async Task<ActionResult<Adress>> GetById(Guid id)
        {
            try
            {
                return Ok(await _adressService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        // POST api/<AdressController>
        [HttpPost("Insert-Adress")]
        public async Task<ActionResult<Adress>> Post([FromBody] Adress obj)
        {
            var result = await _adressService.Create(obj);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return BadRequest("Lỗi");
        }

        // PUT api/<AdressController>/5
        [HttpPut("Update-Adress")]
        public async Task<ActionResult<Adress>> Put(Guid id,Guid UserId ,[FromBody] Adress obj)
        {
            var result = await _adressService.Update(id,UserId, obj);
            if (result)
            {
                return Ok("Sửa thành công");
            }
            return BadRequest("Lỗi");
        }

        // DELETE api/<AdressController>/5
        [HttpDelete("Delete-Adress")]
        public async Task<ActionResult<Adress>> Delete(Guid id, Guid UserId)
        {
            var result = await _adressService.Delete(id,UserId);
            if (result)
            {
                return Ok("Xóa thành công");
            }
            return BadRequest("Lỗi");
        }
    }
}
