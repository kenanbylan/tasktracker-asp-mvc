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
    public IActionResult Events()
    {
        var events = _context.Events.ToList();
        return View(events);
    }

    public IActionResult EventsUser()
    {
        return View();
    }

    public IActionResult EventsJoinUser()
    {
        return View();
    }
}