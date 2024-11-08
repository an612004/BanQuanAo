using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.Encodings.Web;

namespace BanQuanAo.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IRankService _rankService;
        private readonly RoleManager<Role> _roleManager;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,RoleManager<Role> roleManager, IConfiguration configuration,IRankService rankService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _rankService = rankService;
            _roleManager = roleManager;
        }
        // CREATE
        public async Task<bool> Create(UserViewModel item, string roleName)
        {
            var newRank = await _rankService.GetItemByName("Bạc");
            var user = new User()
            {
                UserName = item.UserName,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                DateOfBirth = item.DateOfBirth,
                RankId = newRank.Id,
                Points = 0,
            };
            var result = await _userManager.CreateAsync(user, item.PassWord); // add account
            if (result.Succeeded)
            {
                // add to role
                await _userManager.AddToRoleAsync(user, "User");
                return true;
            }
            return false;
        }
        // DELETE
        public async Task<bool> Delete(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;
        }
        // GET ALL USER
        public async Task<ICollection<User>> GetAll()
        {
            var user =  await _userManager.Users.ToListAsync();
            return user;
        }
        // GET USER
        public async Task<User> GetItem(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<User> GetItemid(Guid userName)
        {
            return await _userManager.FindByIdAsync(userName.ToString());
        }
        // RESET PASSWORD
        public async Task<bool> ResetPassword(string userName, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(user, newPassword);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // CHANGE PASSWORD
        public async Task<bool> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            bool passwordMatch = await _userManager.CheckPasswordAsync(user, currentPassword); // check old password
            if (passwordMatch != null)
            {
                var results = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword); // change password
                if (results.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
       // quên mật khẩu
       
        public async Task<bool> CheckPassword(string userName, string currentPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            bool passwordMatch = await _userManager.CheckPasswordAsync(user, currentPassword); // check old password
            if (passwordMatch)
            {
                return true;


            }
            return false;
        }
        // CHANGE ROLE
        public async Task<bool> ChangeRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                
                var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // UPDATE
        public async Task<bool> Update(UserViewModel item) // tự nhiên có rank khoai ghê
        {
            var user = await _userManager.FindByIdAsync(item.Id.ToString());
            var currentSecurityStamp = user.SecurityStamp;


            user.Email = item.Email;
            user.PhoneNumber = item.PhoneNumber;
            user.DateOfBirth = item.DateOfBirth;
                    
        
            user.SecurityStamp = currentSecurityStamp;
            var result = await _userManager.UpdateAsync(user); // update account
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
