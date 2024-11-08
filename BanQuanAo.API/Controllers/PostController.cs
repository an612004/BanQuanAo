using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("get-posts")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _postService.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Post>> Get(Guid id)
        {
            try
            {
                return Ok(await _postService.GetItem(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }
        [HttpGet("ChangeStatus")]
        public async Task<ActionResult<bool>> UpdateStatus(Guid id, int status)
        {
            try
            {
                var result = await _postService.UpdateStatus(id, status);
                if (result)
                {
                    return Ok("Sửa thành công");
                }
                return BadRequest("Lỗi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Không lấy được dữ liệu");
            }
        }

        [HttpPost("create-post")]
        public async Task<ActionResult<PostVM>> Post([FromForm] PostVM post)
        {
            var result = await _postService.Create(post);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return BadRequest("Lỗi!");
        }
      
        [HttpPut("update-post")]
        public async Task<ActionResult<Post>> Put([FromForm] PostVM post)
        {
            var result = await _postService.Update(post);
            if (result)
            {
                return Ok("Đã thêm thành công");
            }
            return BadRequest("Lỗi!");
        }
        [HttpDelete("delete-post/{id}")]
        public async Task<ActionResult<Post>> Delete(Guid id,Guid UserId)
        {
            var result = await _postService.Delete(id,UserId);
            if (result)
            {
                return Ok("Đã xoá thành công");
            }
            return Ok("Lỗi!");
        }
    }
}
