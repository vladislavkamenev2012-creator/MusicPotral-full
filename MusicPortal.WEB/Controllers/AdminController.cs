using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Interfaces;

namespace MusicPortal.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await _adminService.GetUsersAsync();
            ViewBag.Genres = await _adminService.GetGenresAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int id)
        {
            await _adminService.ApproveUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                await _adminService.AddGenreAsync(name);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            await _adminService.DeleteGenreAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
