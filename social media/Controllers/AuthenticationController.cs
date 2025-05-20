using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using social_media.Data;
using social_media.Data.Constants;
using social_media.Data.Models;
using social_media.Data.Services;
using social_media.ViewModels.Authentication;
using social_media.ViewModels.Settings;
using System.Security.Claims;

namespace social_media.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenService _tokenService;
        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager
            , SignInManager<User> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this._tokenService = tokenService;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var user = await userManager.FindByEmailAsync(loginVM.Email);
            var password = await userManager.CheckPasswordAsync(user, loginVM.Password);
            /*var result = await signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);*/
            if (password)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                await AddCookie(HttpContext, user.Id);
                if (User.IsInRole(AppRoles.Admin))
                {
                    return RedirectToAction("index", "Admin");
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }

                
            
            ModelState.AddModelError("", "invalid login");
            return View(loginVM);
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var newuser = new User()
            {
                FullName = $"{registerVM.FirstName} {registerVM.LastName}",
                Email = registerVM.Email,
                UserName = registerVM.FirstName,
            };

            var existuser = await userManager.FindByEmailAsync(newuser.Email);
            if (existuser != null)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View();
            }

            var result  = await userManager.CreateAsync(newuser , registerVM.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newuser , AppRoles.User);
                await AddCookie(HttpContext, newuser.Id);
                await signInManager.SignInAsync(newuser, isPersistent:true);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete("RefreshToken");
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(PasswordVM passwordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordVM);
            }

            if (passwordVM.NewPassword != passwordVM.ConfirmPassword)
            {
                TempData["PasswordError"] = "Passwords do not match";
                TempData["ActiveTab"] = "Password";

                return RedirectToAction("index", "Settings");
            }

            var loggedinuser = await userManager.GetUserAsync(User);
            var iscorrct = await userManager.CheckPasswordAsync(loggedinuser, passwordVM.CurrentPassword);

            if (!iscorrct)
            {
                TempData["PasswordError"] = "Current Passwords is invalid";
                TempData["ActiveTab"] = "Password";

                return RedirectToAction("index", "Settings");
            }
            var result = await userManager.ChangePasswordAsync(loggedinuser , passwordVM.CurrentPassword , passwordVM.ConfirmPassword);
            if (result.Succeeded)
            {
                TempData["PasswordError"] = "Passwords Update successfully";
                TempData["ActiveTab"] = "Password";

                await signInManager.RefreshSignInAsync(loggedinuser);
                return RedirectToAction("index", "Settings");
            }


            return RedirectToAction("index");
        }

        public IActionResult ExternalLogin(string provider)
        {
            var redirecturl = Url.Action("ExternalLoginCallback");
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider , redirecturl);
            return Challenge(properties,"Google");
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);


            if (user == null)
            {
                var fullname = info.Principal.FindFirstValue(ClaimTypes.Name);
                var username = fullname.Split(" ");
                var newuser = new User()
                {
                    Email = email,
                    FullName = fullname,
                    UserName = username[0],
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newuser);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newuser , AppRoles.User);
                    await signInManager.SignInAsync(newuser, isPersistent: false);

                    return RedirectToAction("Index" , "Home");
                }
            }
            await AddCookie(HttpContext, user.Id);
            await signInManager.SignInAsync(user, isPersistent: false);
            //await AddCookie(HttpContext, user.Id);
            return RedirectToAction("Index" , "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileVM profileVM)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            user.FullName = profileVM.FullName;
            user.UserName = profileVM.UserName;
            user.Bio = profileVM.Bio;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["UserProfileError"] = "User profile could not to update";
                TempData["ActiveTab"] = "Profile";

                return View(profileVM);
            }

            return RedirectToAction("index" , "Settings");
        }

        private async Task AddCookie(HttpContext httpContext , int userId)
        {
            var accessToken = await _tokenService.GenerateJWTTokenAsync(userId);
            var refreshToken = _tokenService.GenerateRefreshToken(userId);

            // تنظیمات کوکی برای AccessToken
            var accessCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            // تنظیمات کوکی برای RefreshToken
            var refreshCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            // اضافه کردن کوکی‌ها به پاسخ
            httpContext.Response.Cookies.Append("AccessToken", accessToken, accessCookie);
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, refreshCookie);

        }
    }
}
