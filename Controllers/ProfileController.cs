using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using tasktracker.Data;
using tasktracker.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tasktracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        // ilgili id girişi yapılarak profil bilgileri getirilir
        [HttpGet]
        public IActionResult ProfileID(string id)
        {
            Console.WriteLine("IDDD : " + id);

            var isAvailable = _context.Profiles.Any(e => e.UserId == id);

            if (isAvailable)
            {
                var profile = _context.Profiles.FirstOrDefault(e => e.UserId == id);
                ViewBag.Profile = profile;
                return View("Profile", profile);
            }
            else
            {
                TempData["ErrorMessage"] = "Profil bulunamadı.";
                return RedirectToAction("Profile");
            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _context.Profiles.Any(e => e.UserId == id);
            if (user == false)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri zaten doldurulmamış.";
                return RedirectToAction("Profile");
            }
            else
            {
                var profile = _context.Profiles.FirstOrDefault(e => e.UserId == id);
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profil bilgileri başarıyla silindi.";
                return RedirectToAction("Profile");
            }
        }


        /*
        [HttpPost]
        public async Task<IActionResult> SaveProfile(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profil başarıyla kaydedildi.";

                return RedirectToAction("Members");
            }
            else
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                TempData["ErrorMessage"] = "Profil kaydedilirken hata oluştu: " + string.Join(", ", errorMessages);

                Console.WriteLine("Error: " + string.Join(", ", errorMessages));
            }

            return View("Profile", profile);
        }
        */

        [HttpPost]
        public async Task<IActionResult> SaveProfile(Profile profile)
        {
            if (ModelState.IsValid)
            {
                // Profil var mı diye kontrol et
                var existingProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == profile.UserId);

                if (existingProfile != null)
                {
                    // Profil varsa güncelle
                    existingProfile.UserName = profile.UserName;
                    existingProfile.UserSurname = profile.UserSurname;
                    existingProfile.UserEmail = profile.UserEmail;
                    existingProfile.UserPhone = profile.UserPhone;
                    existingProfile.UserImage = profile.UserImage;
                    existingProfile.UserBio = profile.UserBio;
                }
                else
                {
                    // Profil yoksa ekle
                    _context.Add(profile);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profil başarıyla kaydedildi.";

                return RedirectToAction("Members");
            }
            else
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                TempData["ErrorMessage"] = "Profil kaydedilirken hata oluştu: " + string.Join(", ", errorMessages);

                Console.WriteLine("Error: " + string.Join(", ", errorMessages));
            }

            return View("Profile", profile);
        }


        [HttpGet]
        public IActionResult Members()
        {
            var members = _context.Profiles.ToList();
            return View(members);
        }
    }
}