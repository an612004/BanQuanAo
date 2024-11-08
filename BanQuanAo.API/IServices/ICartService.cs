using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface ICartService
    {
        public Task<bool> Create(Cart item);
        public Task<Cart> GetItem(Guid id);
    }
}
