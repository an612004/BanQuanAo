using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _iCategoryService;

        public CategoryController(ICategoryService iCategoryService)
        {
            _iCategoryService = iCategoryService;
        }
        [HttpGet("get-all-Category")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _iCategoryService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get-{id}")]
        public async Task<ActionResult<Category>> Get(Guid id)
        {
            try
            {
                return Ok(await _iCategoryService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpPost("create-Category")]
        public async Task<ActionResult<Category>> Post(CategoryVM obj)
        {
            var result = await _iCategoryService.Create(obj);
            return Ok(result);
        }

        [HttpPut("update-Category-{id}")]
        public async Task<ActionResult<Category>> Put(Guid id, [FromBody] Category obj)
        {
            var result = await _iCategoryService.Update(id, obj);
            if (result)
            {
                return Ok("Đã sửa thành công");
            }
            return Ok("Lỗi!");
        }
        [HttpDelete("delete-Category-{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id)
        {
            var result = await _iCategoryService.Delete(id);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
