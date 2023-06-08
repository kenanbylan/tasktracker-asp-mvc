using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using tasktracker.Data;
using tasktracker.Models;

namespace tasktracker.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EventController(ILogger<EventController> logger, ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }


    // EventController.cs

    public IActionResult SearchEvents(string searchText)
    {
        var events = _context.Events.ToList();
        // Modelinizi doğru şekilde filtreleyin ve arama sonuçlarını döndürün
        var filteredEvents = events.Where(e => e.event_name.Trim().ToLower().StartsWith(searchText.Trim().ToLower()))
            .ToList();

        // Görünümü döndürün
        return View("Events", filteredEvents);
    }


    public IActionResult EventsJoinUser()
    {
        return View();

    }

    //Convert Base64 to Image function 
    public byte[] ConvertBase64ToBytes(string base64Image)
    {
        string base64Data = base64Image.Split(',')[1];
        return Convert.FromBase64String(base64Data);
    }


    // Tum etkinlikleri listeler
    public IActionResult Events()
    {
        var events = _context.Events.ToList();
        return View(events);
    }
  
  
    // Aktif Kullaniciya ait olusturulmus etkinlikleri listeler
    public IActionResult EventsUser()
    {
        var events = _context.Events.ToList();
        return (IActionResult)View(events);
    }

    //Aktif Kullanicinin katildigi - katilacaği etkinlikleri listeler
    
    
    public async Task<IActionResult> EventsUserLeave(int eventId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userProfile = _context.Profiles.FirstOrDefault(p => p.UserId == user.Id);
    
        var joinedEventsArray = JsonConvert.DeserializeObject<List<int>>(userProfile.JoinedEvents);
        joinedEventsArray.Remove(eventId);
    
        var updatedJoinedEventsString = JsonConvert.SerializeObject(joinedEventsArray);
        userProfile.JoinedEvents = updatedJoinedEventsString;
    
        await _context.SaveChangesAsync();
    
        return RedirectToAction("Index", "Home");
    }



    //Bir etkinlik ayrintilarini gosterir
    public IActionResult EventView(int id)
    {
        Console.WriteLine("IDDD : " + id);

        var eventItem = _context.Events.FirstOrDefault(e => e.event_id == id);
        if (eventItem == null)
        {
            return NotFound();
        }

        ViewBag.ActivityName = eventItem.event_name;
        ViewBag.ActivityDescription = eventItem.event_description;
        ViewBag.ActivityTime = eventItem.event_date;
        ViewBag.ActivityLocation = eventItem.event_location;
        var userProfile = _context.Profiles.FirstOrDefault(p => p.UserId == eventItem.event_ownerID);
        ViewBag.ActivityOwner = userProfile.UserName + " " + userProfile.UserSurname;

        ViewBag.ActivityRequirements = eventItem.event_requirement;
        ViewBag.ActivityFee = eventItem.event_fee;
        //ViewBag.ActivityContactChannel = eventItem.event_contactChannel;
        ViewBag.Base64ActivityImage = eventItem.event_image;

        return View();
    }

    //Etkinlik Ekleme Sayfasini Gosterir
    public IActionResult EventAdder()
    {
        return View();
    }


    // POST: Event/CreateEvent
    [HttpPost]
    public async Task<ActionResult> SaveEvent(Event model)
    {
        var userId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userId))
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("ProfileAddEventID", "Profile", new { eventId = model.event_id });
            }
            else
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = "Profil kaydedilirken hata oluştu: " + string.Join(", ", errorMessages);
                Console.WriteLine("Error: " + string.Join(", ", errorMessages));
            }
        }
        else
        {
            Console.WriteLine("User ID is empty.");
        }

        return View("EventAdder", model);
    }


    [HttpPost]
    public async Task<IActionResult> JoinEvent(int eventId)
    {
        var user = await _userManager.GetUserAsync(User);
        var userProfile = _context.Profiles.FirstOrDefault(p => p.UserId == user.Id);
        
        // JoinedEvents sütunundaki değeri ayrıştırın
        var joinedEventsArray = JsonConvert.DeserializeObject<List<int>>(userProfile.JoinedEvents);

        // Tıklanan etkinlik ID'sini JoinedEvents dizisine ekleyin
        joinedEventsArray.Add(eventId);

        // Güncellenmiş JoinedEvents dizisini JSON formatına dönüştürün
        var updatedJoinedEventsString = JsonConvert.SerializeObject(joinedEventsArray);

        // Kullanıcının profile tablosunda JoinedEvents sütununu güncelleyin
        userProfile.JoinedEvents = updatedJoinedEventsString;

        // Profil değişikliklerini veritabanına kaydedin
        await _context.SaveChangesAsync();
        
        // İşlem tamamlandığında yönlendirme yapabilir veya başka bir işlem yapabilirsiniz
        return RedirectToAction("Index", "Home");
    }
}