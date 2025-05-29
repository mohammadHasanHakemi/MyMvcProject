using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcProject.Models;
using NAudio.Wave;

namespace MyMvcProject.Controllers.Admin.Musics
{
    [Route("Admin/Musics/[action]/{id?}")]
    public class MusicsController : Controller
    {


        private readonly AppDbContext _context;

        public MusicsController(AppDbContext context)
        {
            _context = context;
            
        }
        public IActionResult Musics()
        {
            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {
        var model = new AlbumMusicViewModel
        {
            Albums = _context.Albums.ToList(),
            Musics = _context.Musics.ToList()
        };

    
            
            return View("~/views/Admin/Musics/Musics.cshtml", model);
               
            }
else{ return RedirectToAction("Login", "Admin");}
        }
        public IActionResult AddMusic()
        {
            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {
                var model = new AddMusic
                {
                    Albums = _context.Albums.ToList(),
                    Music = new Music
                    {
                        Name = "",
                        Property = "",
                        PosterPath = "",
                        MusicPath = "",
                        View = 0,
                        Like = 0,
                        Time = ""
                    }
                };

                return View("~/Views/Admin/Musics/AddMusic.cshtml", model);
            }
else{ return RedirectToAction("Login", "Admin");}
        }
      
        



        // پردازش فرم آپلود
        [HttpPost]
        public async Task<IActionResult> AddMusic(Music music, IFormFile posterFile,IFormFile musicFile)
        {
            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

            if (posterFile == null || posterFile.Length == 0)
            {
                ViewBag.Error = "لطفاً یک فایل عکس انتخاب کنید.";
                return View(music);
            }

            // بررسی حجم (حداکثر 10 مگابایت)
            if (posterFile.Length > 10 * 1024 * 1024)
            {
                ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
                return View(music);
            }

            // بررسی فرمت تصویر
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(posterFile.ContentType.ToLower()))
            {
                ViewBag.Error = "فرمت فایل باید JPG یا PNG باشد.";
                return View(music);
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
            music.PosterPath = fileName;
            
            if (musicFile == null || musicFile.Length == 0)
                {
                    ViewBag.Error = "لطفاً یک فایل عکس انتخاب کنید.";
                    return View(music);
                }

            // بررسی حجم (حداکثر 10 مگابایت)
            if (musicFile.Length > 10 * 1024 * 1024)
            {
                ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
                return View(music);
            }

            // بررسی فرمت تصویر
             allowedTypes = new[] { "audio/mpeg", "audio/wav", "audio/mp3" };
            if (!allowedTypes.Contains(musicFile.ContentType.ToLower()))
            {
                ViewBag.Error = "فرمت فایل باید wav یا mp3 باشد.";
                return View(music);
            }

            // ساخت مسیر ذخیره فایل
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(musicFile.FileName);
            savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", fileName);

            // اطمینان از وجود پوشه
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await musicFile.CopyToAsync(stream);
            }
            // گرفتن مسیر کامل فایل موزیک
            var musicFilePath = savePath;

            // محاسبه مدت زمان فایل موزیک
            TimeSpan duration;
            using (var reader = new AudioFileReader(musicFilePath))
            {
                duration = reader.TotalTime;
            }

            // تبدیل به فرمت "00:00"
            music.Time = duration.ToString(@"mm\:ss");


                // ذخیره فقط نام فایل در دیتابیس

            music.MusicPath = fileName;
            _context.Musics.Add(music);
            await _context.SaveChangesAsync();

            return RedirectToAction("Musics");
             
            }
else{ return RedirectToAction("Login", "Admin");}
        }
public async Task<IActionResult> EditMusic(int id)
{
            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

    var music = await _context.Musics.FindAsync(id);
    if (music == null)
        return NotFound();

    // ساخت ViewModel
    var vm = new AddMusic
    {
        Music = music,
        Albums = await _context.Albums.ToListAsync()
    };

    // ویو را با ViewModel برگردان
    return View("~/Views/Admin/Musics/EditMusic.cshtml", vm);
        
             
            }
            else
            {
               return RedirectToAction("Login", "Admin");
            }
}

[HttpPost]
public async Task<IActionResult> EditMusic(AddMusic vm, IFormFile? posterFile, IFormFile? musicFile)
{
    if (HttpContext.Session.GetString("IsAdmin") != "true")
        return RedirectToAction("Login", "Admin");

    // بارگذاری مجدد لیست آلبوم‌ها در صورت خطا
    vm.Albums = await _context.Albums.ToListAsync();

    // پیدا کردن رکورد اصلی
    var existing = await _context.Musics.FindAsync(vm.Music.Id);
    if (existing == null)
        return NotFound();

    // به‌روزرسانی فیلدهای متنی
    existing.Name     = vm.Music.Name;
    existing.Property = vm.Music.Property;
    existing.AlbumId  = vm.Music.AlbumId;

    // اگر فایل پوستر جدید اومد
    if (posterFile != null && posterFile.Length > 0)
    {
        
            if (posterFile == null || posterFile.Length == 0)
            {
                ViewBag.Error = "لطفاً یک فایل عکس انتخاب کنید.";
                return View(vm);
            }

            // بررسی حجم (حداکثر 10 مگابایت)
            if (posterFile.Length > 10 * 1024 * 1024)
            {
                ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
                return View(vm);
            }

            // بررسی فرمت تصویر
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(posterFile.ContentType.ToLower()))
            {
                ViewBag.Error = "فرمت فایل باید JPG یا PNG باشد.";
                return View(vm);
            }
        // حذف فایل قبلی
                if (!string.IsNullOrEmpty(existing.PosterPath))
                {
                    var old = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", existing.PosterPath);
                    if (System.IO.File.Exists(old))
                    {
                        
                        System.IO.File.Delete(old);
                    }
                }
        // ذخیره فایل جدید
        var imgName = Guid.NewGuid() + Path.GetExtension(posterFile.FileName);
        var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", imgName);
        Directory.CreateDirectory(Path.GetDirectoryName(imgPath)!);
            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                await posterFile.CopyToAsync(stream);
            }
        existing.PosterPath = imgName;
    }

    // اگر فایل موزیک جدید اومد
    if (musicFile != null && musicFile.Length > 0)
    {
                    if (musicFile == null || musicFile.Length == 0)
                {
                    ViewBag.Error = "لطفاً یک فایل عکس انتخاب کنید.";
                    return View(vm);
                }

            // بررسی حجم (حداکثر 10 مگابایت)
            if (musicFile.Length > 10 * 1024 * 1024)
            {
                ViewBag.Error = "حجم فایل نباید بیشتر از 10 مگابایت باشد.";
                return View(vm);
            }

            // بررسی فرمت تصویر
            var  allowedTypes = new[] { "audio/mpeg", "audio/wav", "audio/mp3" };
            if (!allowedTypes.Contains(musicFile.ContentType.ToLower()))
            {
                ViewBag.Error = "فرمت فایل باید wav یا mp3 باشد.";
                return View(vm);
            }
        if (!string.IsNullOrEmpty(existing.MusicPath))
                {
                    var old = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", existing.MusicPath);
                    if (System.IO.File.Exists(old))
                    {
                        
                     System.IO.File.Delete(old);
                    }
                }
        var audioName = Guid.NewGuid() + Path.GetExtension(musicFile.FileName);
        var audioPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", audioName);
        Directory.CreateDirectory(Path.GetDirectoryName(audioPath)!);
        using var audioStream = new FileStream(audioPath, FileMode.Create);
                {
            await musicFile.CopyToAsync(audioStream);
                }

        existing.MusicPath = audioName;
        // گرفتن زمان موزیک
        using var reader = new AudioFileReader(audioPath);
        existing.Time = reader.TotalTime.ToString(@"mm\:ss");
    }

    // ذخیره نهایی
    await _context.SaveChangesAsync();
    return RedirectToAction("Musics");
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteMusic(int id)
{
    var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

    var music = await _context.Musics.FindAsync(id);
    if (music == null)
        return NotFound();

    // حذف فایل کاور در صورت وجود
    if (!string.IsNullOrEmpty(music.PosterPath))
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", music.PosterPath);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
        if (!string.IsNullOrEmpty(music.MusicPath))
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", music.MusicPath);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }

    _context.Musics.Remove(music);
    await _context.SaveChangesAsync();
    return RedirectToAction("Musics");
             
            }
else{ return RedirectToAction("Login", "Admin");}
}

    }
    
    
        
}
