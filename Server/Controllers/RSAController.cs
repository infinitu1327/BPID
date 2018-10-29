using Microsoft.AspNetCore.Mvc;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSAController : ControllerBase
    {
        private readonly IRSAService _rsaService;

        public RSAController(IRSAService rsaService)
        {
            _rsaService = rsaService;
        }

        [HttpGet]
        public IActionResult GetPublicKeys()
        {
            var keys = _rsaService.GetPublicKeys(HttpContext.Connection.Id);
            return Ok(keys);
        }

        [HttpPost]
        public IActionResult Decrypt([FromForm] string encryptedText)
        {
            return Ok(_rsaService.GetDecryptedText(encryptedText, HttpContext.Connection.Id));
        }
    }
}