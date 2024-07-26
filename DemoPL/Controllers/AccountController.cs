using DemoDAL.Models;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
           _signInManager = signInManager;
        }
        #region Sign up 
        [HttpGet]
        public IActionResult SignUp() 
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    user =  await _userManager.FindByEmailAsync(model.Email);
                   if (user == null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            LastName = model.LastName,
                            FirstName = model.FirstName,
                            IsAgree = model.IsAgree,

                        };



                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(SignIn));
                        }
                        else
                        {
                            foreach(var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty,error.Description);
                            }
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "User is already Existed ");
            }
           
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }


            return View(model);
        }
        #endregion

        #region SignOut

        public new async Task<IActionResult> SignOut()
        {
            await  _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region ResetPassword

        public IActionResult ForgetPassword()
        {

            return View();
        }

        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipients = model.Email,
                        Body = url
                    };
                    EmailSettings.SendEmail(email);

                 
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");

            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email ,string token)
        {
            TempData["token"] = token;
            TempData["email"] = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
               var email = TempData["email"] as string;
               var token = TempData["token"] as string;

               var user = await  _userManager.FindByEmailAsync(email);
                if(user is not null)
                 {
                      var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                      if (result.Succeeded)
                       {
                           return RedirectToAction(nameof(SignIn));

                       }
                         else
                           {
                              foreach(var error in result.Errors)
                                 {
                                      ModelState.AddModelError(string.Empty,error.Description);
                                 }
                           }

                }

                ModelState.AddModelError(string.Empty, "Invalid Reset Password");
            }
            return View(model);
        }
        #endregion
        #region AccesDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
