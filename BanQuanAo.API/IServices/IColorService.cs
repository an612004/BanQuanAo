using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface IColorService
    {
        public Task<bool> Create(Colors item);

        public Task<bool> Delete(Guid id);

        public Task<List<Colors>> GetAll();

        public Task<Colors> GetItem(Guid id);

        public Task<bool> Update(Guid id, Colors item);
    }
}
