using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_media.Data.Constants;
using social_media.Data.Services;

namespace social_media.Controllers
{
    [Authorize(Roles =AppRoles.User)]
    public class ChatController : Controller
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }
        public async Task<IActionResult> Index(int id)
        {
            var messages = await _chatMessageService.GetContectsAsync(id);
            return View(messages);
        }

        public async Task<IActionResult> Private(int id)
        {
            var messages = await _chatMessageService.Messages(id);
            ViewBag.UserGoal = id;
            return View(messages);
        }
    }
}
