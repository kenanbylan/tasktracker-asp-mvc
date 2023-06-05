using Microsoft.AspNetCore.Mvc;
using tasktracker.Data;

namespace tasktracker.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly ApplicationDbContext _context;
    
    public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    // GET
    public IActionResult Profile()
    {
        return View();
    }
    
    
    // GET
    public IActionResult Members()
    {
        var members = _context.Profiles.ToList();
        return View(members);
    }
}