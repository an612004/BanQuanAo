using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface ISizeService
    {
        public Task<bool> Create(SizeVM item);

        public Task<bool> CreateMany(List<SizeVM> items);

        public Task<bool> Delete(Guid id);

        //public Task<bool> DeleteMany(List<> items);

        public Task<IEnumerable<Size>> GetAll();

        public Task<Size> GetItem(Guid id);

        public Task<bool> Update(Guid id, SizeVM item);

        //public Task<bool> UpdateMany(List<> items);
    }
}
