using EcommerceWeb.Data;
using EcommerceWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _dbContext;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View(_dbContext.ApplicationUsers.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                _dbContext.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult>Edit(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var editUser = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if(editUser == null)
            {
                return NotFound();
            }
            editUser.FirstName = user.FirstName;
            editUser.LastName = user.LastName;
            editUser.PhoneNumber = user.PhoneNumber;
            editUser.Address = user.Address;

            var result = await _userManager.UpdateAsync(editUser);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(editUser);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _dbContext.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _dbContext.ApplicationUsers.FindAsync(id);
            _dbContext.ApplicationUsers.Remove(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
