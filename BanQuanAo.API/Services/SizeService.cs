using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class SizeService : ISizeService
    {
        private MyDbContext _dbContext;


        public SizeService(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }

        public async Task<bool> Create(SizeVM item)
        {
            try
            {
                Size size = new Size()
                {
                    Id = Guid.NewGuid(),
                    SizeName = item.SizeName,
                    Height = item.Height,
                    Width = item.Width
                };
                await _dbContext.AddAsync(size);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> CreateMany(List<SizeVM> items)
        {
            try
            {
                var lstS = new List<Size>();
                foreach (var x in items)
                {
                    Size size = new Size()
                    {
                        Id = Guid.NewGuid(),
                        Width = x.Width,
                        Height = x.Height,
                        SizeName = x.SizeName,
                    };
                    lstS.Add(size);
                }
                await _dbContext.Size.AddRangeAsync(lstS);
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
                var result = await _dbContext.Size.FindAsync(id);
                _dbContext.Remove(result);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Size>> GetAll()
        {
            return await _dbContext.Size.ToListAsync();
        }

        public async Task<Size> GetItem(Guid id)
        {
            var item = await _dbContext.Size.FindAsync(id);
            return item;
        }

        public async Task<bool> Update(Guid id, SizeVM item)
        {
            var result = await _dbContext.Size.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            else
            {
                result.SizeName = item.SizeName;
                result.Width = item.Width;
                result.Height = item.Height;

                await _dbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
