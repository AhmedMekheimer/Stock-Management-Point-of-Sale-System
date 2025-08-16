﻿using CoreLayer.Models;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PresentationLayer.Areas.IDentitiy.ViewModels;

namespace PresentationLayer.Areas.IDentitiy.controller
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository<ApplicationUserOTP> _appUserOTP;
        private async Task SendConfirmationEmail(ApplicationUser applicationUser)
        {
            //Generating Email Conf. Token 
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

            //Link sent to Email, to be redirected to 'action' that verifies token 
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = applicationUser.Id, token = token, area = "Identity" }, Request.Scheme);

            //Sending Confirmation Email to New Account
            await _emailSender.SendEmailAsync(applicationUser.Email!, "Account Confirmation", $"<h1>Confirm Your Account By Clicking <a href='{link}'>here</a></h1>");
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM LoginVM)
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
                    if (!user.EmailConfirmed)
                    {
                        TempData["error-notification"] = "Confirm Your Email, The Confirmation Email has been Sent";
                        await SendConfirmationEmail(user);
                        return View(LoginVM);
                    }
                    if (!user.LockoutEnabled)
                    {
                        TempData["error-notification"] = $"You are locked out until {user.LockoutEnd}";
                        return View(LoginVM);
                    }

                    await _signInManager.SignInAsync(user, LoginVM.RememberMe);
                    TempData["success-notification"] = "Signed In Successfully";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                ModelState.AddModelError("UserNameOrEmail", "Invalid UserName Or Email");
                ModelState.AddModelError("Password", "Invalid Password");
                return View(LoginVM);
            }

            ModelState.AddModelError("UserNameOrEmail", "Invalid UserName Or Email");
            ModelState.AddModelError("Password", "Invalid Password");
            return View(LoginVM);
        }


        //  email confirmation
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                    TempData["success-notification"] = "Confirm Email Successfully";
                else
                    TempData["error-notification"] = $"{String.Join(",", result.Errors)}";

                return RedirectToAction("SignIn", "Account", new { area = "Identity" });
            }
            return NotFound();
        }
        public IActionResult EmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationVM emailConfirmationVM)
        {
            if (!ModelState.IsValid)
                return View(emailConfirmationVM);

            var user = await _userManager.FindByNameAsync(emailConfirmationVM.UserNameOrEmail) ??
                       await _userManager.FindByEmailAsync(emailConfirmationVM.UserNameOrEmail);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    TempData["success-notification"] = "The Confirmation Email has been Sent";
                    await SendConfirmationEmail(user);
                }
                else
                    TempData["error-notification"] = "Your Email is Confirmed";
                return RedirectToAction("SignIn", "Account", new { area = "Identity" });
            }
            TempData["error-notification"] = "Email or UserName Invalid";
            return View(emailConfirmationVM);
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
                if (!user.EmailConfirmed)
                {
                    TempData["success-notification"] = "Your Account needs Confirmation. The Confirmation Email has been Sent";
                    await SendConfirmationEmail(user);
                    return RedirectToAction("SignIn", "Account", new { area = "Identity" });
                }

                // Send Reset Password OTP Email
                var otpNumber = new Random().Next(1000, 9999);

                var totalNumberOfOTPs = (await _appUserOTP.GetAsync(e => e.ApplicationUserId == user.Id ));

                if (totalNumberOfOTPs.Count() > 5)
                {
                    TempData["error-notification"] = "Many Requests of OTPs";
                    return View(forgetPasswordVM);
                }

                await _appUserOTP.CreateAsync(new()
                {
                    ApplicationUserId = user.Id,
                    OTPNumber = otpNumber,
                    Status = true,
                    ValidTo = DateTime.UtcNow.AddMinutes(30)
                });

                await _emailSender.SendEmailAsync(user!.Email ?? "", "OTP Your Account", $"<h1>Reset Password using OTP: {otpNumber}</h1>");

                TempData["success-notification"] = "OTP sent to your Email Successfully";

                return RedirectToAction("ResetPassword", "Account", new { area = "Identity", UserId = user.Id });
            }
            TempData["error-notification"] = "Email or UserName Invalid";
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
                var lastOTP = (await _appUserOTP.GetAsync(e => e.ApplicationUserId == resetPasswordVM.UserId)).OrderBy(e => e.Id).LastOrDefault();

                if (lastOTP is not null && lastOTP.OTPNumber == resetPasswordVM.OTPNumber && lastOTP.Status && lastOTP.ValidTo > DateTime.UtcNow)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

                    if (result.Succeeded)
                    {
                        TempData["success-notification"] = "Password Reset Succussfully";
                        return RedirectToAction("SignIn", "Account", new { area = "Identity" });
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
                else
                    TempData["error-notification"] = "OTP is Invalid or Expired";

                return View(resetPasswordVM);
            }
            return NotFound();
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
                {
                    // Check if email is already in use
                    var emailExists = await _userManager.FindByEmailAsync(profileEditVM.Email);
                    if (emailExists != null)
                    {
                        ModelState.AddModelError("Email", "Email is already in use.");
                        return View(profileEditVM);
                    }
                }
                if (profileEditVM.UserName != user.UserName)
                {
                    // Check if username is already in use
                    var userExists = await _userManager.FindByNameAsync(profileEditVM.UserName);
                    if (userExists != null)
                    {
                        ModelState.AddModelError("UserName", "Username is already in use.");
                        return View(profileEditVM);
                    }
                }
                if (profileEditVM.Email != user.Email)
                    user.EmailConfirmed = false;
                user.UserName = profileEditVM.UserName;

                user.Email = profileEditVM.Email;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Refresh authentication cookie
                    await _signInManager.RefreshSignInAsync(user);

                    TempData["success-notification"] = "Profile is Updated Successfully";
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return NotFound();
        }
    }
}