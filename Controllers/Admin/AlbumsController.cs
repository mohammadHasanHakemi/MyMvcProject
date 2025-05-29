using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcProject.Models;

namespace MyMvcProject.Controllers.Admin.Albums
{
    [Route("Admin/Albums/[action]/{id?}")]
    public class AlbumsController : Controller
    {


        private readonly AppDbContext _context;

        public AlbumsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Albums()
        {
                        var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

        var model = new AlbumMusicViewModel
        {
            Albums = _context.Albums.ToList(),
            Musics = _context.Musics.ToList()
        };
            return View("~/views/Admin/Albums/Albums.cshtml", model);
               
            }
else{ return RedirectToAction("Login", "Admin");}
        }

        public IActionResult AddAlbum()
        {
                        var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

             
            return View("~/views/Admin/Albums/AddAlbum.cshtml");
            }
else{ return RedirectToAction("Login", "Admin");}
            // "~/views/Admin/Albums/AddAlbums.cshtml"
        }
        // پردازش فرم آپلود
        [HttpPost]
        public async Task<IActionResult> AddAlbum(Album album, IFormFile posterFile)
        {
                                    var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

            if (posterFile == null || posterFile.Length == 0)
            {
                ViewBag.Error = "لطفاً یک فایل عکس انتخاب کنید.";
                return View(album);
            }

            // بررسی حجم (حداکثر 10 مگابایت)
            if (posterFile.Length > 10 * 1024 * 1024)
            {
                ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
                return View(album);
            }

            // بررسی فرمت تصویر
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(posterFile.ContentType.ToLower()))
            {
                ViewBag.Error = "فرمت فایل باید JPG یا PNG باشد.";
                return View(album);
            }

            // ساخت مسیر ذخیره فایل
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(posterFile.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", fileName);

            // اطمینان از وجود پوشه
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await posterFile.CopyToAsync(stream);
            }

            // ذخیره فقط نام فایل در دیتابیس
            album.PosterPath = fileName;

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return RedirectToAction("Albums");
             
            }
else{ return RedirectToAction("Login", "Admin");}
        }
public async Task<IActionResult> EditAlbum(int id)
{
                            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

    var thisAlbum = await _context.Albums.FindAsync(id);
    if (thisAlbum == null)
        return NotFound();

    return View("~/views/Admin/Albums/EditAlbum.cshtml", thisAlbum);
             
            }
else{ return RedirectToAction("Login", "Admin");}
}


        [HttpPost]
        public async Task<IActionResult> EditAlbum(int id, Album album, IFormFile? posterFile)
        {
                            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

             
    if (id != album.Id)
        return BadRequest();

    var existingAlbum = await _context.Albums.FindAsync(id);
    if (existingAlbum == null)
        return NotFound();

    // به‌روزرسانی نام آلبوم
    existingAlbum.Name = album.Name;

    // اگر فایل جدید انتخاب شده
    if (posterFile != null && posterFile.Length > 0)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
        if (!allowedTypes.Contains(posterFile.ContentType.ToLower()))
        {
            ViewBag.Error = "فرمت فایل باید JPG یا PNG باشد.";
            return View("~/Views/Admin/Albums/EditAlbum.cshtml", existingAlbum);
        }

        if (posterFile.Length > 10 * 1024 * 1024)
        {
            ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
            return View("~/Views/Admin/Albums/EditAlbum.cshtml", existingAlbum);
        }

        // حذف عکس قبلی
        if (!string.IsNullOrEmpty(existingAlbum.PosterPath))
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", existingAlbum.PosterPath);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
        }

        // ذخیره فایل جدید
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(posterFile.FileName);
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await posterFile.CopyToAsync(stream);
        }

        existingAlbum.PosterPath = fileName;
    }

    await _context.SaveChangesAsync();
    return RedirectToAction("Albums");
            }
else{ return RedirectToAction("Login", "Admin");}
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteAlbum(int id)
{
                            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

    var album = await _context.Albums.FindAsync(id);
    if (album == null)
        return NotFound();

    // حذف فایل کاور در صورت وجود
    if (!string.IsNullOrEmpty(album.PosterPath))
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", album.PosterPath);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }

    _context.Albums.Remove(album);
    await _context.SaveChangesAsync();
    return RedirectToAction("Albums");
             
            }
else{ return RedirectToAction("Login", "Admin");}
}

    }
    
    
        
}
