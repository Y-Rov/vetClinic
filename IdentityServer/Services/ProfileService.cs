using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;


    public ProfileService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IUserClaimsPrincipalFactory<User> claimsFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _claimsFactory = claimsFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        if (sub is null) throw new InvalidCredentialException("User must contain sub claim");
        User user = await _userManager.FindByIdAsync(sub);
        ClaimsPrincipal userClaimsPrincipal = await _claimsFactory.CreateAsync(user);
        Claim roleClaim = userClaimsPrincipal.Claims.Single(c => c.Type == "role");
        context.IssuedClaims.Add(roleClaim);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        User user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}