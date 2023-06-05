using Microsoft.AspNetCore.Mvc;
using tasktracker.Data;

namespace tasktracker.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;
    private readonly ApplicationDbContext _context;

    public EventController(ILogger<EventController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    // GET
    
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
        var message = "Events";
        var number = 2;
        ViewData["Message"] = message;
        ViewData["Number"] = number;
        

        var array = new[] {"one", "two", "three"};
        ViewData["Array"] = array;
        
        return (IActionResult) View(events);
    }

    //Aktif Kullanicinin katildigi - katilacaği etkinlikleri listeler
    public IActionResult EventsJoinUser()
    {
        var events = _context.Events.ToList();
        return View(events);
    }
    
    
    //Bir etkinlik ayrintilarini gosterir
    public IActionResult EventView()
    {
        return View();
    }
    
    //Etkinlik Ekleme Sayfasini Gosterir
    public IActionResult EventAdder()
    {
        return View();
    }
}