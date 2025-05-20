using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Data.Constants;
using social_media.Data.Services;

namespace social_media.Controllers
{
    [Authorize(Roles =AppRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
                _adminService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            var posts = await _adminService.GetReportedPostAsync();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReport(int postId)
        {
           await _adminService.ApproveReportAsync(postId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectReport(int postId)
        {
            await _adminService.RejectReportAsync(postId);

            return RedirectToAction("Index");
        }
    }
}
