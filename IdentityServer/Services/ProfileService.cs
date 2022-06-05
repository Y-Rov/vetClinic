using System.Security.Claims;
using Core.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
    
    public ProfileService(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsFactory)
    {
        _userManager = userManager;
        _claimsFactory = claimsFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        User user = await _userManager.FindByIdAsync(sub);
        ClaimsPrincipal userClaimsPrincipal = await _claimsFactory.CreateAsync(user);
        
        var requestedClaims = 
            context.RequestedResources.Resources.IdentityResources.SelectMany(r => r.UserClaims);
        
        // Add issued claims (if "profile" scope is included) and role claim by default
        List<Claim> claimsToAdd = userClaimsPrincipal.Claims
            .Where(claim => requestedClaims.Contains(claim.Type) || claim.Type is "role")
            .ToList();
        
        context.IssuedClaims.AddRange(claimsToAdd);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        User user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user.IsActive;
    }
}