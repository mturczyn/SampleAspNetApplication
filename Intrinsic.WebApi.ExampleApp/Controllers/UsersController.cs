using Intrinsic.WebApi.ExampleApp.AuthManagement;
using Intrinsic.WebApi.ExampleApp.UserManagement;
using Intrinsic.WebApi.ExampleApp.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Intrinsic.WebApi.ExampleApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IRepository _userRepository;

    public UsersController(IRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("/login")]
    public async Task<IActionResult> LoginAsync(
        [FromQuery] string[] neededPermissions)
    {
        var identity = new ClaimsIdentity(
            neededPermissions
                .Select(x => new Claim(
                    PermissionsAuthRequirement.ClaimType, 
                    x)),
            CookieAuthenticationDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim("is-super-intrinsic-user", "true"));

        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(identity);

        await HttpContext.SignInAsync(claimsPrincipal);
        return Ok();
    }

    [HttpGet("/create")]
    [PermissionAuth(
        PermissionOperator.And, 
        UserManagement.Permission.Create)]
    public async Task<IActionResult> CreateAsync()
    {
        return Ok(new { Result = "Created!" });
    }

    [HttpGet("/logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet("getOwnClaims")]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsersAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _userRepository.GetUsersAsync(cancellationToken));
    }
}
