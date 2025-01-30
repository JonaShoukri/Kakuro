using Microsoft.AspNetCore.Mvc;
using KakuroWEB.Models;

namespace KakuroWEB.Controllers;
public class AccountController: Controller
{
    
    [AuthorizeUser]
    public IActionResult User()
    {
        return View();
    }
}