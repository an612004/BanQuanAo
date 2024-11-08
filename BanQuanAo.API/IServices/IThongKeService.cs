using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IThongKeService
    {
       Task< ThongKeViewModel> ThongKe(string startDate, string endDate);
    }
}
