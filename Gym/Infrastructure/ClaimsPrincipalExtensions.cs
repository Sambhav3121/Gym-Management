using System;
using System.Security.Claims;

namespace Gym.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue("UserId")
                     ?? user.FindFirstValue(ClaimTypes.NameIdentifier) 
                     ?? user.FindFirstValue("sub"); 

            return Guid.Parse(id!);
        }
    }
}
