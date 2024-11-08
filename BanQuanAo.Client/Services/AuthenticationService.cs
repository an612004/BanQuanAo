using BanQuanAo.Client.IServices;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace BanQuanAo.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Response> Login(LoginViewModel model,string url)
        {
            //var url = "https://localhost:7007/api/authentication/login";
            Response response = new Response();
            var result = await _httpClient.PostAsJsonAsync(url, model);
            
            if (result.IsSuccessStatusCode)
            {
                response.IsSuccess = true;
                response.Messages = "Login Success";
                response.Token = await result.Content.ReadAsStringAsync();
            }
            else
            {
                response.IsSuccess = false;
                response.Messages = "Login Fail";
            }
            return response;
        }

        public async Task<Response> Logout()
        {
            var url = "https://localhost:7007/api/authentication/logout";
            Response response = new Response();
            var result = await _httpClient.GetAsync(url);
            response.IsSuccess = false;
            if (result.IsSuccessStatusCode)
            {
                response.IsSuccess = true;
                response.Messages = result.RequestMessage.ToString();

            }
            
            return response;
        }


        public async Task<Response> Register(RegisterViewModel model, string url)
        {
            Response response = new Response();
            var result = await _httpClient.PostAsJsonAsync(url, model);

            if (result.IsSuccessStatusCode)
            {
                response.IsSuccess = true;
                response.Messages = "Register Success";

            }
            else
            {
                response.IsSuccess = false;
                response.Messages = "Login Fail";
            }
            return response;
        }
    }
}
