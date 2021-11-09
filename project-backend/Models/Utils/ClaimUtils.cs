using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace project_backend.Models.Utils
{
    public static class ClaimUtils
    {
        public static Claim getUserIdClaim(HttpContext httpContext)
        {
            return (httpContext.User.Identity as ClaimsIdentity).Claims.First(claim => claim.Type == ClaimCtxTypes.Id);
        }
    }
}
