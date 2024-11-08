using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.Client.IServices
{
    public interface IAuthenticationService
    {
        public Task<Response> Login(LoginViewModel model, string url);
        public Task<Response> Register(RegisterViewModel model, string url);
        public Task<Response> Logout();
    }
}
