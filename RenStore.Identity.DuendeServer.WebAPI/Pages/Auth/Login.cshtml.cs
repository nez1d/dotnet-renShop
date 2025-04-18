/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;
using RenStore.Identity.DuendeServer.WebAPI.ViewModels;
using System.Security.Claims;
using RenStore.Domain.Models;
using RenStore.Identity.DuendeServer.WebAPI.Service;

namespace RenStore.Identity.DuendeServer.WebAPI.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly JwtProvider jwtProvider;
    private readonly UserService userService;

    public LoginModel(SignInManager<ApplicationUser> signInManager,
                      UserManager<ApplicationUser> userManager,
                      UserService userService) =>
        (this.signInManager, this.userManager, this.jwtProvider,
            this.userService) =
        (signInManager, userManager, jwtProvider = new JwtProvider(),
            userService);

    [BindProperty]
    public LoginViewModel model { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found!");
                return Page();
            }

            var data = await userService.CheckEmailConfirmed(model.Email);
            
            if (!data)
                return BadRequest("Email is not confirmed!");

            var result = await userService.Login(user.Email, model.Password);
            
            if (!result)
                return BadRequest("Error while logging in!");

            /*var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, "AuthUser")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims: claims, 
                authenticationType: "pwd", 
                nameType: ClaimTypes.Name, 
                roleType: ClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var token = jwtProvider.GenerateToken(user);

            this.HttpContext.Response.Cookies.Append("tasty-cookies", token);

            await this.HttpContext.SignInAsync(claimsPrincipal);#1#

            return Redirect("https://localhost:7005/index.html");
        }
        return Page();
    }
}*/