using Microsoft.AspNetCore.Mvc;
using KakuroWEB.Models;

namespace KakuroWEB.Controllers;

[AuthorizeUser]
public class AccountController: Controller
{
    public IActionResult User()
    {
        return View();
    }
}