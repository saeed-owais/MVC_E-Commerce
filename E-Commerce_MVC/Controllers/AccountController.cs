using AutoMapper;
using DA.Models;
using DAL.ViewModels;
using E_Commerce_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var existingEmail = await userManager.FindByEmailAsync(registerViewModel.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "This Email Is Already Registered");
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
                    UserName = registerViewModel.Email,
                    FullName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    PhoneNumber = registerViewModel.Phone,
                    Address = registerViewModel.Country
                };

                var result = await userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login");

                }

                foreach (var error in result.Errors)
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
            if (!ModelState.IsValid)
                return View("Login", loginViewModel);

            var appUser = await userManager.FindByEmailAsync(loginViewModel.Email);
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

        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            var profileViewModel = new ProfileViewModel
            {
                Name = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Status = user.IsDeleted ? "Disabled" : "Active",
                JoinDate = user.CreatedOnUtc,
                ImageURL = user.ImageURL,
                Address = user.Address
            };

            return View(profileViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var editProfileViewModel = new EditProfileViewModel
            {
                Name = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address,
                ExistingImageURL = user.ImageURL
            };

            return View(editProfileViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveEdits(EditProfileViewModel editProfileViewModel)
        {
            if (!ModelState.IsValid)
                return View("Edit", editProfileViewModel);

            var user = await userManager.GetUserAsync(User);

            var emailExists = await userManager.Users.AnyAsync(u => u.Email == editProfileViewModel.Email && u.Id != user.Id);
            if (emailExists)
                ModelState.AddModelError("Email", "This Email Is Already Registered");

            var phoneExists = await userManager.Users.AnyAsync(u => u.PhoneNumber == editProfileViewModel.Phone && u.Id != user.Id);
            if (phoneExists)
                ModelState.AddModelError("Phone", "This Phone Is Already Registered");

            if (!ModelState.IsValid)
                return View("Edit", editProfileViewModel);

            user.FullName = editProfileViewModel.Name;
            user.Email = editProfileViewModel.Email;
            user.PhoneNumber = editProfileViewModel.Phone;
            user.Address = editProfileViewModel.Address;

            if (editProfileViewModel.ImageFile != null && editProfileViewModel.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(editProfileViewModel.ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await editProfileViewModel.ImageFile.CopyToAsync(stream);
                }

                user.ImageURL = "/images/users/" + fileName;
            }

            user.ImageURL ??= editProfileViewModel.ExistingImageURL;

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Profile");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("Edit", editProfileViewModel);
        }


        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        public async Task<IActionResult> SaveNewPassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }


            var result = await userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "The Old Paswword is not Correct!");
                return View("ChangePassword");
            }

            TempData["Success Massage"] = "Password Updated Successfully";
            return RedirectToAction("Profile");
        }
    }
}

