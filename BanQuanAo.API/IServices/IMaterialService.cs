using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IMaterialService
    {
        public Task<bool> Create(MaterialVM item);

        public Task<bool> Delete(Guid id);

        public Task<List<Material>> GetAll();

        public Task<Material> GetItem(Guid id);

        public Task<bool> Update(Guid id, MaterialVM item);
    }
}
