using Microsoft.AspNetCore.Mvc;
using social_media.ViewModels.SideBar;
using System.Security.Claims;

public class SidebarViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {       
        var userId = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var model = new SideBarVM
        {
            UserId = int.Parse(userId)
        };
        return View(model); 
    }
}