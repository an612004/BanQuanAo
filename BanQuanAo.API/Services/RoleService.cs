using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Create(Role Role)
        {
            var result = await _roleManager.CreateAsync(Role);
            return result.Succeeded;
        }

        public async Task<bool> Delete(string roleName)
        {
            var IdentityRole = await GetItem(roleName);
            var result = await _roleManager.DeleteAsync(IdentityRole);
            return result.Succeeded;
        }

        public async Task<ICollection<Role>> GetAll()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetItem(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);

        }

        public async Task<bool> Update(Role role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
