using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.DTOs;
using MusicPortal.BLL.Interfaces;
using MusicPortal.WEB.Models;
using System.Security.Claims;

namespace MusicPortal.WEB.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService _songService;
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _env;

        public SongsController(ISongService songService, IAdminService adminService, IWebHostEnvironment env)
        {
            _songService = songService;
            _adminService = adminService;
            _env = env;
        }

        public async Task<IActionResult> Index(string? search, int? genreId, string sortOrder = "title", int page = 1)
        {
            int pageSize = 3;
            var (songs, totalCount) = await _songService.GetSongsAsync(search, genreId, sortOrder, page, pageSize);
            var genres = await _adminService.GetGenresAsync();

            var vm = new SongListViewModel
            {
                Songs = songs,
                Genres = genres,
                Search = search,
                GenreId = genreId,
                SortOrder = sortOrder,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalCount
                }
            };

            return View(vm);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _adminService.GetGenresAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSongViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filePath = "/uploads/sample.mp3";
                if (model.AudioFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AudioFile.FileName);
                    string path = Path.Combine(_env.WebRootPath, "uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.AudioFile.CopyToAsync(stream);
                    }
                    filePath = "/uploads/" + fileName;
                }

                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "1");

                var dto = new SongDTO
                {
                    Title = model.Title,
                    Artist = model.Artist,
                    GenreId = model.GenreId!.Value,
                    FilePath = filePath,
                    UserId = userId
                };

                await _songService.AddSongAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = await _adminService.GetGenresAsync();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _songService.DeleteSongAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
