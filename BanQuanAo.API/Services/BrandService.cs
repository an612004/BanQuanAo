using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class BrandService : IBrandService
    {
        public MyDbContext _dbContext;

        public BrandService(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }
        public async Task<bool> Create(BrandVM item)
        {
            try
            {
                var brand = new Brand()
                {
                        BrandName = item.BrandName,
                };
                await _dbContext.Brand.AddAsync(brand);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var item = await _dbContext.Brand.FirstOrDefaultAsync(c => c.Id == id);
                _dbContext.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

       
        public async Task<List<Brand>> GetAll()
        {
            return await _dbContext.Brand.ToListAsync();
        }
        public async Task<Brand> GetItem(Guid id)
        {
            return await _dbContext.Brand.FindAsync(id);

        }

        public async Task<bool> Update(Guid id, BrandVM item)
        {
            try
            {
                var brand = await _dbContext.Brand.FirstOrDefaultAsync(c => c.Id == id);

                brand.BrandName = item.BrandName;

                _dbContext.Brand.Update(brand);
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
