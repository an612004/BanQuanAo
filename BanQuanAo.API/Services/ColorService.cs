using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class ColorService : IColorService
    {
        private readonly MyDbContext _dbContext;

        public ColorService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Colors item)
        {
            try
            {
                var color = new Colors()
                {
                    ColorId = Guid.NewGuid(),
                    ColorCode = item.ColorCode,
                    ColorName = item.ColorName,
                };
                await _dbContext.Colors.AddAsync(color);
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
                var item = await _dbContext.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
                _dbContext.Colors.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Colors>> GetAll()
        {
            return await _dbContext.Colors.ToListAsync();
        }

        public async Task<Colors> GetItem(Guid id)
        {
            return await _dbContext.Colors.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, Colors item)
        {
            try
            {
                var colorForcus = await _dbContext.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
                colorForcus.ColorCode = item.ColorCode;
                colorForcus.ColorName = item.ColorName;
                _dbContext.Colors.Update(colorForcus);
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
