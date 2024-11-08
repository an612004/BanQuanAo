using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface IAdressService
    {
        public Task<bool> Create(Adress item);

        public Task<bool> Delete(Guid id, Guid UserId);

        public Task<List<Adress>> GetAll();

        public Task<Adress> GetItem(Guid id);

        public Task<bool> Update(Guid id, Guid UserId, Adress item);
    }
}
