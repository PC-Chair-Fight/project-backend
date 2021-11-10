using System.Security.Claims;
using System.Linq;

namespace project_backend.Models.Utils
{
    public static class ClaimUtils
    {
        public static Claim GetUserIdClaim(this ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).Claims.First(claim => claim.Type == ClaimCtxTypes.Id.ToString());
        }
    }
}
