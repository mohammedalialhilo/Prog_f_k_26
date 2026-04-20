using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    
    public class TestController : ApiBaseController
    {
        
        [Authorize()]
        [HttpGet("secure")]
        public ActionResult GetSecureInfo()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok($"Ditt username {username} och ditt userId {userId}");
        }
    }
}
