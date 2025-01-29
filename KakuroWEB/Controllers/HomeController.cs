using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KakuroWEB.Models;
using Microsoft.AspNetCore.Authorization;

namespace KakuroWEB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [AuthorizeUser]
    public IActionResult Dashboard()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}