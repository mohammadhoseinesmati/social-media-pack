using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using social_media.Data.Models;
using System.Net.Mail;
using System.Security.Claims;

namespace social_media.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected int? GetUserId()
        {
            var loggeduserid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggeduserid == null)
                return null;
            return int.Parse(loggeduserid);
        }

        protected IActionResult GoToLogin()
        {
            return RedirectToAction("Login" , "Authentication");
        }

        protected string? FullName()
        {
            var loggeduserid = User.FindFirstValue(ClaimTypes.Name);
            if (loggeduserid == null)
            {
                return "";
            }
            return loggeduserid;
        }
    }
}
