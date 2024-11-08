using BanQuanAo.API.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
	[Route("api/cart")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private ICartService _iCartService;
		public CartController(ICartService iCartService)
		{
			_iCartService = iCartService;
		}
		[HttpGet("get-item-Cart")]
		public async Task<IActionResult> GetItem(Guid userId)
		{
			try
			{
				return Ok(await _iCartService.GetItem(userId));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return StatusCode(500, "Không lấy được dữ liệu");
			}
		}
	}
}
