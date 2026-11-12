using System.Security.Claims;
using MefaOzelEgitim.Domain.Interfaces;
using MefaOzelEgitim.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MefaOzelEgitim.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Kullanıcının mevcut Claim'lerini çekiyoruz
            var claims = await _userManager.GetClaimsAsync(user);
            var firstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value ?? string.Empty;
            var lastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value ?? string.Empty;

            var model = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                FirstName = firstName,
                LastName = lastName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userManager.FindByIdAsync(model.Id);
            if (existingUser == null)
            {
                return NotFound();
            }
            
            existingUser.UserName = model.UserName;
            existingUser.Email = model.Email;
            existingUser.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            var existingClaims = await _userManager.GetClaimsAsync(existingUser);

            var oldFirstNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FirstName");
            if (oldFirstNameClaim != null)
            {
                await _userManager.ReplaceClaimAsync(existingUser, oldFirstNameClaim, new Claim("FirstName", model.FirstName));
            }
            else
            {
                await _userManager.AddClaimAsync(existingUser, new Claim("FirstName", model.FirstName));
            }
            var oldLastNameClaim = existingClaims.FirstOrDefault(c => c.Type == "LastName");
            if (oldLastNameClaim != null)
            {
                await _userManager.ReplaceClaimAsync(existingUser, oldLastNameClaim, new Claim("LastName", model.LastName));
            }
            else
            {
                await _userManager.AddClaimAsync(existingUser, new Claim("LastName", model.LastName));
            }

            TempData["Success"] = "Kullanıcı bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Error"] = "Admin yetkisine sahip hesaplar sistemden silinemez!";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}