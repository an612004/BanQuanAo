using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IWishListService
    {
        public Task<bool> Create(Guid UserId, Guid ProductId);

        public Task<bool> Delete(Guid Id);

        public Task<List<WishListVM>> GetAll();

        public Task<WishList> GetItem(Guid Id);

    }
}
