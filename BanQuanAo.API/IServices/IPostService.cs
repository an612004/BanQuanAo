using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IPostService
    {
        public Task<bool> Create(PostVM item);

        public Task<bool> Delete(Guid id, Guid UserId);

        public Task<List<Post>> GetAll();

        public Task<Post> GetItem(Guid id);
        public Task<bool> UpdateStatus(Guid id,int status);

        public Task<bool> Update(PostVM item);
    }
}
