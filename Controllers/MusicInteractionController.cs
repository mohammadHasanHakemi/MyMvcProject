
using Microsoft.AspNetCore.Mvc; // وارد کردن فضای نام برای کنترلرها و اکشن‌ها
using MyMvcProject.Models; // وارد کردن فضای نام برای مدل‌ها
using Microsoft.EntityFrameworkCore; // وارد کردن فضای نام برای کار با EF Core
using System.Threading.Tasks; // وارد کردن فضای نام برای کار با Task

namespace MyMvcProject.Controllers // تعریف نام‌فضای کنترلرها
{
    [Route("api/music")] // تنظیم مسیر پایه برای تمام اکشن‌ها (مثلاً /api/music/...)
    [ApiController] // مشخص کردن که این یک کنترلر API است
    public class MusicInteractionController : Controller // تعریف کنترلر که از ControllerBase ارث‌بری می‌کنه
    {
        private readonly AppDbContext _context; // تعریف فیلد برای دسترسی به دیتابیس

        public MusicInteractionController(AppDbContext context) // سازنده کنترلر برای تزریق وابستگی
        {
            _context = context; // مقداردهی فیلد context با پارامتر ورودی
        }

        [HttpPost("{musicId}/like")] // تعریف اکشن برای درخواست POST با پارامتر musicId در مسیر
        public async Task<IActionResult> ToggleLike(int musicId, [FromHeader] string deviceId) // اکشن برای تoggle کردن لایک
        {
            if (string.IsNullOrEmpty(deviceId)) // چک کردن اینکه deviceId خالی نباشد
                return BadRequest("DeviceId is required"); // برگرداندن خطا اگه deviceId خالی باشه

            using var transaction = await _context.Database.BeginTransactionAsync(); // شروع تراکنش برای هماهنگی داده‌ها
            try
            {
                var interaction = await _context.MusicInteractions // جستجوی تعامل موجود با MusicId و DeviceId
                    .FirstOrDefaultAsync(i => i.MusicId == musicId && i.DeviceId == deviceId);

                Music? music = await _context.Musics.FindAsync(musicId); // پیدا کردن موزیک مربوطه (فقط یه بار تعریف می‌کنیم)
                if (music == null) return NotFound("Music not found"); // برگرداندن خطا اگه موزیک پیدا نشد

                if (interaction == null) // اگه تعاملی پیدا نشد
                {
                    interaction = new MusicInteraction // ایجاد یه نمونه جدید از MusicInteraction
                    {
                        MusicId = musicId, // تنظیم MusicId با مقدار ورودی
                        DeviceId = deviceId, // تنظیم DeviceId با مقدار هدر
                        IsLiked = true, // تنظیم اولیه لایک به true
                        IsViewed = false, // تنظیم اولیه ویو به false
                        CreatedAt = DateTime.UtcNow, // تنظیم زمان ایجاد به الان
                        Music = music // تنظیم پراپرتی Music که required هست
                    };
                    await _context.MusicInteractions.AddAsync(interaction); // اضافه کردن تعامل جدید به دیتابیس با روش async
                }
                else // اگه تعامل پیدا شد
                {
                    interaction.IsLiked = !interaction.IsLiked; // تoggle کردن وضعیت لایک (true به false یا برعکس)
                }

                // از همون متغیر music استفاده می‌کنیم، دیگه تعریف نمی‌کنیم
                if (music != null) // اگه موزیک پیدا شده بود (که هست، چون قبلاً چک کردیم)
                {
                    music.Like += interaction.IsLiked ? 1 : -1; // افزایش یا کاهش تعداد لایک‌ها (ستون Like توی دیتابیس)
                }

                await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
                await transaction.CommitAsync(); // تأیید تراکنش
                return Ok(new { IsLiked = interaction.IsLiked, Likes = music.Like }); // برگرداندن نتیجه با وضعیت و تعداد لایک
            }
            catch (Exception ex) // مدیریت خطاها
            {
                await transaction.RollbackAsync(); // لغو تراکنش در صورت خطا
                return StatusCode(500, $"Internal server error: {ex.Message}"); // برگرداندن خطای سرور با پیام
            }
        }

        [HttpPost("{musicId}/view")] // تعریف اکشن برای درخواست POST با پارامتر musicId در مسیر
        public async Task<IActionResult> RecordView(int musicId, [FromHeader] string deviceId) // اکشن برای ثبت ویو
        {
            if (string.IsNullOrEmpty(deviceId)) // چک کردن اینکه deviceId خالی نباشد
                return BadRequest("DeviceId is required"); // برگرداندن خطا اگه deviceId خالی باشه

            using var transaction = await _context.Database.BeginTransactionAsync(); // شروع تراکنش برای هماهنگی داده‌ها
            try
            {
                var interaction = await _context.MusicInteractions // جستجوی تعامل موجود با MusicId و DeviceId
                    .FirstOrDefaultAsync(i => i.MusicId == musicId && i.DeviceId == deviceId);

                Music? music = await _context.Musics.FindAsync(musicId); // پیدا کردن موزیک مربوطه (فقط یه بار تعریف می‌کنیم)
                if (music == null) return NotFound("Music not found"); // برگرداندن خطا اگه موزیک پیدا نشد

                if (interaction == null) // اگه تعاملی پیدا نشد
                {
                    interaction = new MusicInteraction // ایجاد یه نمونه جدید از MusicInteraction
                    {
                        MusicId = musicId, // تنظیم MusicId با مقدار ورودی
                        DeviceId = deviceId, // تنظیم DeviceId با مقدار هدر
                        IsLiked = false, // تنظیم اولیه لایک به false
                        IsViewed = true, // تنظیم اولیه ویو به true
                        CreatedAt = DateTime.UtcNow, // تنظیم زمان ایجاد به الان
                        Music = music // تنظیم پراپرتی Music که required هست
                    };
                    await _context.MusicInteractions.AddAsync(interaction); // اضافه کردن تعامل جدید به دیتابیس با روش async
                }
                else if (!interaction.IsViewed) // اگه تعامل هست ولی ویو نشده
                {
                    interaction.IsViewed = true; // تنظیم وضعیت ویو به true
                }
                else // اگه قبلاً ویو شده
                {
                    return Ok(new { IsViewed = true, Views = music.View }); // برگرداندن وضعیت بدون تغییر
                }

                // از همون متغیر music استفاده می‌کنیم
                if (music != null) // اگه موزیک پیدا شده بود (که هست، چون قبلاً چک کردیم)
                {
                    music.View += 1; // افزایش تعداد ویوها (ستون View توی دیتابیس)
                }

                await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
                await transaction.CommitAsync(); // تأیید تراکنش
                return Ok(new { IsViewed = interaction.IsViewed, Views = music.View }); // برگرداندن نتیجه با وضعیت و تعداد ویو
            }
            catch (Exception ex) // مدیریت خطاها
            {
                await transaction.RollbackAsync(); // لغو تراکنش در صورت خطا
                return StatusCode(500, $"Internal server error: {ex.Message}"); // برگرداندن خطای سرور با پیام
            }
        }

        [HttpGet("{musicId}/status")] // تعریف اکشن برای درخواست GET با پارامتر musicId در مسیر
        public async Task<IActionResult> GetStatus(int musicId, [FromHeader] string deviceId) // اکشن برای گرفتن وضعیت
        {
            if (string.IsNullOrEmpty(deviceId)) // چک کردن اینکه deviceId خالی نباشد
                return BadRequest("DeviceId is required"); // برگرداندن خطا اگه deviceId خالی باشه

            var interaction = await _context.MusicInteractions // جستجوی تعامل موجود با MusicId و DeviceId
                .FirstOrDefaultAsync(i => i.MusicId == musicId && i.DeviceId == deviceId);

            var music = await _context.Musics.FindAsync(musicId); // پیدا کردن موزیک مربوطه
            if (music == null) return NotFound("Music not found"); // برگرداندن خطا اگه موزیک پیدا نشد

            if (interaction == null) // اگه تعاملی پیدا نشد
            {
                return Ok(new // برگرداندن وضعیت پیش‌فرض
                {
                    IsLiked = false, // لایک نشده
                    IsViewed = false, // ویو نشده
                    Likes = music.Like, // تعداد لایک‌ها
                    Views = music.View // تعداد ویوها
                });
            }

            return Ok(new // برگرداندن وضعیت تعامل
            {
                IsLiked = interaction.IsLiked, // وضعیت لایک
                IsViewed = interaction.IsViewed, // وضعیت ویو
                Likes = music.Like, // تعداد لایک‌ها
                Views = music.View // تعداد ویوها
            });
        }
    }
}