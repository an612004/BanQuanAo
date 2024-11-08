using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Runtime.Intrinsics.Arm;

namespace BanQuanAo.API.Services
{
    public class PostService : IPostService
    {
        private readonly MyDbContext _dbContext;

        public PostService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(PostVM item)
        {
            try
            {
                if (item.filecollection != null)//không null 
                {

                    // Bỏ qua file này nếu đã xử lý
                     var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/postimages");
                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        string imgPath = Path.Combine(path, item.filecollection.FileName);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            await item.filecollection.CopyToAsync(stream);
                        }



                        var post = new Post()
                        {
                            Id = Guid.NewGuid(),
                            UserId = item.UserId,
                            Tittle = item.Tittle,
                            TittleImage = "/postimages/" + item.filecollection.FileName,
                            Contents = item.Contents,
                            CreateAt = item.CreateAt,
                            UpdateAt = item.UpdateAt,
                            Description = item.Description,
                            Status = item.Status
                        };
                        await _dbContext.Posts.AddAsync(post);
                    await _dbContext.SaveChangesAsync();

                    return true;

                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid id, Guid UserId)
        {
            try
            {
                    var item = await _dbContext.Posts.FirstOrDefaultAsync(c=>c.Id==id&&c.UserId==UserId);
                    _dbContext.Posts.Remove(item);
                    await _dbContext.SaveChangesAsync();
                    return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Post>> GetAll()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public async Task<Post> GetItem(Guid id)
        {
            return await _dbContext.Posts.FindAsync(id);
        }
        public async Task<bool> UpdateStatus(Guid id, int status)
        {
            try
            {
                var posts = await _dbContext.Posts.FindAsync(id);
                posts.Status = status;
                _dbContext.Posts.Update(posts);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update( PostVM item)
        {
            try
            {
                if (item.filecollection != null)//không null 
                {

                    // Bỏ qua file này nếu đã xử lý
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/postimages");
                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string imgPath = Path.Combine(path, item.filecollection.FileName);
                    using (var stream = new FileStream(imgPath, FileMode.Create))
                    {
                        await item.filecollection.CopyToAsync(stream);
                    }

                    var postForcus = await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == item.Id);
                    postForcus.Tittle = item.Tittle;
                    //postForcus.UserId = item.UserId; // không sửa người tạo
                    postForcus.TittleImage = "/postimages/" + item.filecollection.FileName;
                    postForcus.Contents = item.Contents;
                    postForcus.CreateAt = item.CreateAt;
                    postForcus.UpdateAt = item.UpdateAt;
                    postForcus.Description = item.Description;
                    postForcus.Status = item.Status;
                    _dbContext.Posts.Update(postForcus);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
