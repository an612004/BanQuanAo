using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class AdressService : IAdressService
    {
        private readonly MyDbContext _dbContext;

        public AdressService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Adress item)
        {
            try
            {
                var adress = new Adress()
                {
                    UserId = item.UserId,
                    Province = item.Province,
                    District = item.District,
                    Ward = item.Ward,
                };
                await _dbContext.Adresses.AddAsync(adress);
                await _dbContext.SaveChangesAsync();
                return true;
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
                var item = await _dbContext.Adresses.FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);
                _dbContext.Adresses.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Adress>> GetAll()
        {
            return await _dbContext.Adresses.ToListAsync();
        }

        public async Task<Adress> GetItem(Guid id)
        {
            return await _dbContext.Adresses.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, Guid UserId, Adress item)
        {
            try
            {
                var adressForcus = await _dbContext.Adresses.FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);
                adressForcus.Province = item.Province;
                adressForcus.District = item.District;
                adressForcus.Ward = item.Ward;
                _dbContext.Adresses.Update(adressForcus);
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
