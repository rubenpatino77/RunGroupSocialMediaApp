using System;
using System.Security.Claims;

namespace RunGroupSocialMedia
{
	public static class ClaimsPrincipalExtension
	{
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

