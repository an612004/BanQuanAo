using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface ICategoryService
    {
        public Task<Category> Create(CategoryVM item);

        public Task<bool> Delete(Guid id);

        public Task<List<Category>> GetAll();

        public Task<Category> GetItem(Guid id);

        public Task<bool> Update(Guid id, Category item);
    }
}
