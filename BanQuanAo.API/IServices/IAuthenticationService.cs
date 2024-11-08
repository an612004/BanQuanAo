using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IAuthenticationService
    {
        public Task<Response> Login(LoginViewModel model);
        public Task<Response> Register(RegisterViewModel model);
        public Task Logout();

    }
}
