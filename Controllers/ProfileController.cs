using Microsoft.AspNetCore.Mvc;

namespace tasktracker.Controllers;

public class ProfileController : Controller
{
    // GET
    public IActionResult Profile()
    {
        return View();
    }
}