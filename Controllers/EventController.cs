using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tasktracker.Data;

namespace tasktracker.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EventController(ILogger<EventController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        var filteredEvents = events.Where(e => e.event_name.Trim().ToLower().StartsWith(searchText.Trim().ToLower())).ToList();
    
        // Görünümü döndürün
        return View("Events", filteredEvents);

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
        return (IActionResult) View(events);
    }

    //Aktif Kullanicinin katildigi - katilacaği etkinlikleri listeler
    public IActionResult EventsJoinUser()
    {
        var events = _context.Events.ToList();
        return View(events);
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
        ViewBag.ActivityOwner = eventItem.event_ownerID;
        ViewBag.ActivityRequirements = eventItem.event_requirement;
        ViewBag.ActivityFee = eventItem.event_fee;
        //ViewBag.ActivityContactChannel = eventItem.event_contactChannel;
        //ViewBag.Base64ActivityImage = "";
        
        return View();
    }
    
    //Etkinlik Ekleme Sayfasini Gosterir
    public IActionResult EventAdder()
    {
        return View();
    }
}