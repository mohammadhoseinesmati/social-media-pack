using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using social_media.Controllers.Base;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Helpers;
using social_media.Data.Models;
using social_media.Data.Services;
using social_media.Hubs;
using social_media.ViewModels.Home;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace social_media.Controllers
{
    [Authorize(Roles = AppRoles.User)]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _dbContext;
        private readonly IPostService _postService;
        private readonly IHashtagService _hashtagService;
        private readonly UserManager<User> _userManager;
        
        private readonly INotificationService _notificationService;

        public HomeController(ILogger<HomeController> logger, AppDBContext dbContext
            , IPostService postService , IHashtagService hashtagservice
            ,UserManager<User> usermanager , IHubContext<NotificationHub> hubContext
            , INotificationService notificationService
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _postService = postService;
            _hashtagService = hashtagservice;
            _userManager = usermanager;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            int loggeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var posts = await _postService.GetAllPostsAsync(loggeduser);

            return View(posts);
        }

        public async Task<IActionResult> GetPostById(int postid)
        {
            var post = await _postService.GetPostByIdAsync(postid);
            return View("Details", post);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            int logeduser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var newpost = new Post()
            {
                Created = DateTime.UtcNow,
                Content = post.Content,
                Updated = DateTime.UtcNow,
                ImageUrl = "",
                NrOfReports = 0,
                UserId = logeduser,
                User = await _userManager.FindByIdAsync(logeduser.ToString())
            };

            await _postService.CreatePostAsync(newpost, post.image);

            await _hashtagService.ProccessHashtagForNewPostAsync(newpost.Content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postlikevm)
        {
            var loggeduser = GetUserId();
            var response = await _postService.TogglePostLikeAsync(postlikevm.PostID, loggeduser.Value);

            var fullname = FullName();

            var post = await _postService.GetPostByIdAsync(postlikevm.PostID);

            if (response.SendNotification)
            {
                await _notificationService.AddNewNotficition(post.UserId, NotificationType.Like ,fullname , postlikevm.PostID);
            }

            return PartialView("Home/_Post" , post);

        }
        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibityVM postVisibityVM)
        {
            var loggeduser = GetUserId();
            await _postService.TogglePostVisibilityAsync(postVisibityVM.PostId, loggeduser.Value);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            var loggeduser = GetUserId() ;
            await _postService.AddPostReportAsync(postReportVM.PostId, loggeduser.Value);
            
            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            var loggeduser = GetUserId();

            var newcomment = new Comment()
            {
                PostId = postCommentVM.PostId,
                UserId = loggeduser.Value,
                Content = postCommentVM.Content,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            await _postService.AddCommentPostAsync(newcomment);
            var fullname = FullName();

            var post = await _postService.GetPostByIdAsync(postCommentVM.PostId);

            await _notificationService.AddNewNotficition(post.UserId , NotificationType.Comment, fullname , postCommentVM.PostId);
            //await _postService.AddCommentPostAsync(newcomment);

            return PartialView("Home/_Post", post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postService.RemoveCommentPostAsync(removeCommentVM.commentId);

            var post = await _postService.GetPostByIdAsync(removeCommentVM.postId);
            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {
            var post = await _postService.RemovePostAsync(postRemoveVM.PostId);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            var loggeduser = GetUserId();
            var result = await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggeduser.Value);

            var fullname = FullName();
            
            var post = await _postService.GetPostByIdAsync(postFavoriteVM.PostId);

            if (result.SendNotification)
            {
                await _notificationService.AddNewNotficition(post.UserId, NotificationType.Favorite, fullname , postFavoriteVM.PostId);
            }
            return PartialView("Home/_Post", post); ;

        }

        public async Task<IActionResult> Details(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            return View(post);
        }
    }
}
