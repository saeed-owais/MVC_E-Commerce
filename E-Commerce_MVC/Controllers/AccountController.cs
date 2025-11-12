using AutoMapper;
using DA.Models;
using E_Commerce_MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // var registerDTO = mapper.Map<RegisterDTO>(registerViewModel);
                var existingEmail = await userManager.FindByEmailAsync(registerViewModel.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "This Email Is Already Registerd");
                    return View("Register", registerViewModel);
                }
                var existingPhone = userManager.Users.FirstOrDefault(u => u.PhoneNumber == registerViewModel.Phone);
                if (existingPhone != null)
                {
                    ModelState.AddModelError("Phone", "This Phone Number Is Already Existing!");
                    return View("Register", registerViewModel);
                }

                var user = new ApplicationUser
                {
                    UserName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.Phone,
                };

                var Result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (Result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View("Register", registerViewModel);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginViewModel loginViewModel)
        {
            {
                if (!ModelState.IsValid)
                {
                    return View("Login", loginViewModel);
                }

                ApplicationUser appUser = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (appUser != null)
                {
                    var found = await userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(appUser, loginViewModel.Remmember);
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("Password", "Password is incorrect");
                    return View("Login", loginViewModel);
                }

                ModelState.AddModelError("Email", "Email is incorrect");
                return View("Login", loginViewModel);
            }


        }

            //if (ModelState.IsValid)
            //   {
            //    ApplicationUser appUser = await userManager.FindByEmailAsync(loginViewModel.Email);
            //        if (appUser != null) 
            //        {
            //            var found = await userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
            //            if (found)
            //            {
            //                 await signInManager.SignInAsync(appUser, loginViewModel.Remmember);
            //                 return RedirectToAction("Index", "Home");
            //            }
            //        }
            //    ModelState.AddModelError(string.Empty , "Email or Password isn't Valid!");
            //   }





        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
