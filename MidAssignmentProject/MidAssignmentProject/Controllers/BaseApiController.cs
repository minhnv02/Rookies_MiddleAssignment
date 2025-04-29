using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MidAssignmentProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected string UserID => FindClaim(ClaimTypes.NameIdentifier);

        private string FindClaim(string claimName)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(claimName);

            return claim?.Value ?? string.Empty;
        }
    }
}
