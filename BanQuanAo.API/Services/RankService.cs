using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class RankService : IRankService
    {

        private readonly MyDbContext _dbContext;

        public RankService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Rank item)
        {
            try
            {
                var rank = new Rank()
                {
                    Name = item.Name,
                    PointsMin = item.PointsMin,
                    PoinsMax = item.PoinsMax,
                    Description = item.Description,
                };
                await _dbContext.Rank.AddAsync(rank);
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
                var item = await _dbContext.Rank.FirstOrDefaultAsync(c => c.Id == id);
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

        public async Task<List<Rank>> GetAll()
        {
            return await _dbContext.Rank.ToListAsync();
        }

        public async Task<Rank> GetItem(Guid id)
        {
            return await _dbContext.Rank.FindAsync(id);
        }

        public async Task<Rank> GetItemByName(string name)
        {
            return await _dbContext.Rank.FirstOrDefaultAsync(c=>c.Name.ToLower()==name.ToLower());
        }

        public async Task<bool> Update(Guid id, Rank rank)
        {
            try
            {
                var rankForcus = await _dbContext.Rank.FirstOrDefaultAsync(c => c.Id == id);

                rankForcus.Name = rank.Name;
                rankForcus.PointsMin = rank.PointsMin;
                rankForcus.PoinsMax = rank.PoinsMax;
                rankForcus.Description = rank.Description;

                 _dbContext.Rank.Update(rankForcus);
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
