using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PresentationLayer.Areas.Identity.ViewModels;

namespace PresentationLayer.Areas.Identity.controller
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _UnitOfWork;
        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, IUnitOfWork UnitOfWork)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Login()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "DashBoard" });
            } else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM LoginVM)
        {
        
                if (!ModelState.IsValid)
                    return View(LoginVM);

                var user = await _userManager.FindByNameAsync(LoginVM.UserNameOrEmail) ??
                           await _userManager.FindByEmailAsync(LoginVM.UserNameOrEmail);

                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, LoginVM.Password);

                    if (result)
                    {
                        await _signInManager.SignInAsync(user, LoginVM.RememberMe);
                        TempData["Success"] = "Logged In Successfully";
                        return RedirectToAction("Index", "Home", new { area = "DashBoard" });
                    }

                    ModelState.AddModelError("UserNameOrEmail", "Invalid UserName Or Email");
                    ModelState.AddModelError("Password", "Invalid Password");
                    return View(LoginVM);
                }

                ModelState.AddModelError("UserNameOrEmail", "Invalid UserName Or Email");
                ModelState.AddModelError("Password", "Invalid Password");
                return View(LoginVM);
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);

            var user = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail) ??
                       await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);

            if (user != null)
            {
                // Send Reset Password OTP Email
                var otpNumber = new Random().Next(1000, 9999);

                var totalNumberOfOTPs = (await _UnitOfWork.ApplicationUserOTPs.GetAsync(e => e.ApplicationUserId == user.Id ));

                if (totalNumberOfOTPs.Count() > 500)
                {
                    TempData["Error"] = "Many Requests of OTPs";
                    return View(forgetPasswordVM);
                }

                await _UnitOfWork.ApplicationUserOTPs.CreateAsync(new()
                {
                    ApplicationUserId = user.Id,
                    OTPNumber = otpNumber,
                    Status = true,
                    ValidTo = DateTime.UtcNow.AddMinutes(30)
                });

                await _emailSender.SendEmailAsync(user!.Email ?? "", "OTP Your Account", $"<h1>Reset Password using OTP: {otpNumber}</h1>");

                TempData["Success"] = "OTP sent to your Email Successfully";

                return RedirectToAction("ResetPassword", "Account", new { area = "Identity", UserId = user.Id });
            }
            TempData["Error"] = "Email or UserName Invalid";
            return View(forgetPasswordVM);
        }

        public async Task<IActionResult> ResetPassword(string UserId)
        {
            if (await _userManager.FindByIdAsync(UserId) is ApplicationUser user)
            {
                return View(new ResetPasswordVM()
                {
                    UserId = UserId
                });
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (await _userManager.FindByIdAsync(resetPasswordVM.UserId) is ApplicationUser user)
            {
                if (!ModelState.IsValid)
                    return View(resetPasswordVM);
                var lastOTP = (await _UnitOfWork.ApplicationUserOTPs.GetAsync(e => e.ApplicationUserId == resetPasswordVM.UserId)).OrderBy(e => e.Id).LastOrDefault();

                if (lastOTP is not null && lastOTP.OTPNumber == resetPasswordVM.OTPNumber && lastOTP.Status && lastOTP.ValidTo > DateTime.UtcNow)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Password Reset Succussfully";
                        return RedirectToAction("Login", "Account", new { area = "Identity" });
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
                else
                    TempData["Error"] = "OTP is Invalid or Expired";

                return View(resetPasswordVM);
            }
            return NotFound();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = $"Signed Out Successfully";
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        public async Task<IActionResult> ProfileEdit()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null) return NotFound();

            return View(new ProfileEditVM()
            {
                UserId = currentUser.Id,
                UserName = currentUser.UserName!,
                
                Email = currentUser.Email!,
                
            });
        }

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ProfileEditVM profileEditVM)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Verify user can only edit their own profile
            if (currentUser == null || currentUser.Id != profileEditVM.UserId)
            {
                return Forbid();
            }

            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return View(profileEditVM);

            if (await _userManager.FindByIdAsync(profileEditVM.UserId) is ApplicationUser user)
            {
                if (profileEditVM.Email != user.Email)
                    user.EmailConfirmed = false;
                user.UserName = profileEditVM.UserName;

                user.Email = profileEditVM.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Refresh authentication cookie
                    await _signInManager.RefreshSignInAsync(user);

                    TempData["Success"] = "Profile is Updated Successfully";
                    return RedirectToAction("Index", "Home", new { area = "DashBoard" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(profileEditVM);
                }
            }
            return NotFound();
        }
    }
}