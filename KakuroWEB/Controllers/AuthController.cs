using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KakuroWEB.Controllers;

public class AuthController : Controller
{
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(string email, string password)
    {
        // Simulate successful login 
        // REPLACE
        HttpContext.Session.SetString("IsLoggedIn", "true");
        return RedirectToAction("Dashboard", "Home");
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data
        return RedirectToAction("Login");
    }
    
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    
    public IActionResult ReRegister()
    {
        return View();
    }
}