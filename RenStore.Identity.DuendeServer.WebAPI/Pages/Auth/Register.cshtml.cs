/*
using Duende.IdentityServer.Stores.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RenStore.Domain.Models;
using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;
using RenStore.Identity.DuendeServer.WebAPI.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using RenStore.Identity.DuendeServer.WebAPI.Data.Helpers;
using RenStore.Identity.DuendeServer.WebAPI.Service;

namespace RenStore.Identity.DuendeServer.WebAPI.Pages;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly EmailService emailService;
    private readonly UserService userService;

    public RegisterModel(UserManager<ApplicationUser> userManager, 
        EmailService emailService,
        UserService userService) =>
        (this.userManager, this.emailService, this.userService) = 
        (userManager, emailService, userService);
    

    [BindProperty]
    public RegisterViewModel model { get; set; }
    
    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {    
                UserName = model.Email,
                Email = model.Email
            };
            
            var result = await userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                
                var confirmationLink = Url.Page("ConfirmEmail", "Auth",
                    new { email = user.Email, code = code },
                    HttpContext.Request.Scheme);
                
                await emailService.SendEmailAsync(user.Email, "ConfirmEmail", 
                    $"<a href='{confirmationLink}'>link</a>");

                bool emailStatus = await userManager.IsEmailConfirmedAsync(user);
                
                if (!emailStatus)
                    return BadRequest("Check your email.");

                var loginResult = await userService.Login(user.Email, user.PasswordHash);

                if (!loginResult)
                    return BadRequest("User email confirmation failed.");        
                
                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, model.Email),
                    new (ClaimTypes.Role, "AuthUser"),
                    new ("UserId", user.Id)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "pwd", ClaimTypes.Name, ClaimTypes.Role);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal);

                var token = jwtProvider.GenerateToken(user);

                this.HttpContext.Response.Cookies.Append("tasty-cookies", token);

                await this.HttpContext.SignInAsync(claimsPrincipal);

                return Redirect("http://localhost:5185/index.html");
            }
            return BadRequest("User Creation Failed");
        }
        return Page();
    }


/*    public dynamic GetToken()
    {
        var handler = new JwtSecurityTokenHandler();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.KEY));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var identity = new ClaimsIdentity(new GenericIdentity("tasty-cookies"), 
            new[] { new Claim(ClaimTypes.Role, "AuthUser") });
        var token = handler.CreateJwtSecurityToken(subject: identity,
                                                   signingCredentials: signingCredentials,
                                                   audience: "ExampleAudience",
                                                   issuer: "ExampleIssuer",
                                                   expires: DateTime.UtcNow.AddHours(12));
        return handler.WriteToken(token);
    }#1#
}
*/
