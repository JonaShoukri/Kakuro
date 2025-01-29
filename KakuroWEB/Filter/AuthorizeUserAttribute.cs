using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace KakuroWEB.Models;

public class AuthorizeUserAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var isLoggedIn = context.HttpContext.Session.GetString("IsLoggedIn");

        if (string.IsNullOrEmpty(isLoggedIn))
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }

        base.OnActionExecuting(context);
    }
}