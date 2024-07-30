﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using ShopDevelop.Web.Models;
using ShopDevelop.Data.DataBase;
using ShopDevelop.Data.Repository.Entity;
using ShopDevelop.Data.Repository.Interfaces;
using ShopDevelop.Service.Interfaces;
using ShopDevelop.Service.Entity;
using Microsoft.Net.Http.Headers;

namespace ShopDevelop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RegisterUserRepository _registerUserRepository;
        private readonly UserRepository _userRepository;
        private readonly UserModelView _userModel;
        private readonly IPasswordHasher _PasswordHasher;
        private readonly JwtProvider _jwtProvider;
        private HttpContext context;

        public AccountController(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _registerUserRepository = new RegisterUserRepository(context, passwordHasher);
            _userRepository = new UserRepository(context);
            _userModel = new UserModelView();
        }   

        public async Task<IActionResult> UserProfile(UserModelView model)
        {
            if (model == null)
            {
                return View("Login");
            }
            
            return View(_userModel);
        }
        
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var register = _registerUserRepository
                .RegisterUserByEmail(model.Login, model.Password, model.Email);

            if (await register == true)
            {
                var pass = RepeatPassword(model);
                if (await pass == true)
                {
                    var userProfileModel = _userRepository.GetUserForLogin(model.Login);
                    _userModel.User = await userProfileModel;

                    SetClaim(model.Login);
                    return View("UserProfile", _userModel);
                }
            }
            return RedirectToAction("Register");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.User
                .FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

            if (user == null) 
            {
                ModelState.AddModelError("", "User not found");
            }
            else if(user.Login == model.Login || user.Password == model.Password)
            {
                SetClaim(model.Login);

                /*var token = _jwtProvider.GenerateToken(user);*/
                /*context.Response.Cookies.Append("tasty-cookies", token);*/

                var userProfileModel = _userRepository.GetUserForLogin(model.Login);
                _userModel.User = await userProfileModel;

                return View("UserProfile", _userModel);
            }
            return View();
        }

        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync("Cookie");
            return Redirect("/Home/Index");
        }

        public async Task<bool> RepeatPassword(RegisterModelView model)
        {
            if(model.Password == null || model.Password != model.RepeatPassword)
            {
                return false;
            }
            return true;
        }

        public async void SetClaim(string name)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name)
            };

            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("Cookie", claimsPrincipal);
        }

        public async Task<IActionResult> GetClaim(string name)
        {
            /*var accessToken = Request.Headers[HeaderNames.Authorization];
            var cookie = await HttpContext.GetTokenAsync("Cookie", accessToken);
            return Redirect("UserProfile");*/
        }
    }
}