using BanQuanAo.API.IServices;
using BanQuanAo.API.Services;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService _thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }
        [HttpGet("ThongKe")]
        public async Task< ThongKeViewModel> ThongKe(string startDate, string endDate)
        {
            return await _thongKeService.ThongKe(startDate, endDate);
        }
    }
}
