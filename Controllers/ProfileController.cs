using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using tasktracker.Data;
using tasktracker.Models;
using System.Threading.Tasks;

namespace tasktracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
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
        
        
        [HttpPost]
        public async Task<IActionResult> SaveProfile(Profile profile)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    profile.UserId = user.Id;
                    Console.WriteLine("User ID: " + profile.UserId);

                    _context.Add(profile);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Profil başarıyla kaydedildi.";

                    return RedirectToAction("Members");
                }
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