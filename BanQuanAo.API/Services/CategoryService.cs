using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace BanQuanAo.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly MyDbContext _dbContext;

        public CategoryService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> Create(CategoryVM item)
        {
            try
            {
                var cate = await _dbContext.Category.FindAsync(item.Id);
                //check ton tai
                var exist = await _dbContext.Category.Where(c=>c.CategoryName.ToLower().Trim() == item.CategoryName.ToLower().Trim() && c.Id != item.Id).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return null;
                }
                if(cate != null)
                {
                    cate.CategoryName = item.CategoryName;
                    cate.IdCategory = item.IdCategoryPa;
                    _dbContext.Category.Update(cate);
                    await _dbContext.SaveChangesAsync();
                    return cate;
                }
                else
                {
                    Category ct = new Category()
                    {
                        Id = new Guid(),
                        CategoryName = item.CategoryName,
                        IdCategory = item.IdCategoryPa
                    };
                    await _dbContext.Category.AddAsync(ct);
                    await _dbContext.SaveChangesAsync();
                    return ct;
                }
                
               
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var item = await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id);
                if (_dbContext.Product.Any(c => c.CategoryId == id))
                {
                    return false;
                }
                _dbContext.Category.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Category>> GetAll()
        {
            return await _dbContext.Category.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetItem(Guid id)
        {
            return await _dbContext.Category.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, Category item)
        {
            try
            {
                var category = await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id);
                category.CategoryName = item.CategoryName;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
