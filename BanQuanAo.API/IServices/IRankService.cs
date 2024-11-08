using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface IRankService
    {
        public Task<bool> Create(Rank item);

        public Task<bool> Delete(Guid id);

        public Task<List<Rank>> GetAll();

        public Task<Rank> GetItem(Guid id);
        public Task<Rank> GetItemByName(string name);

        public Task<bool> Update(Guid id,Rank rank);
    }
}
