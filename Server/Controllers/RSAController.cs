using System;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class RSAController : Controller
    {
        private readonly IRSAService _rsaService;

        public RSAController(IRSAService rsaService)
        {
            _rsaService = rsaService;
        }

        // GET api/values/id
        [HttpGet("{id}")]
        public IActionResult Get([FromHeader] int? id)
        {
            if (!id.HasValue) return new BadRequestResult();

            return new OkObjectResult(_rsaService.GetPublicKeys(id.Value));
        }

        // POST api/values/id
        [HttpPost("{id}")]
        public IActionResult Post([FromForm] string encryptedText, int? id)
        {
            if (!id.HasValue || encryptedText == null) return new BadRequestResult();

            Console.WriteLine("Encrypted text (base 64):");
            Console.WriteLine(encryptedText);

            var decryptedText = _rsaService.GetDecryptedText(encryptedText, id.Value);

            Console.WriteLine("Decrypted text:");
            Console.WriteLine(decryptedText);

            return new OkObjectResult(decryptedText);
        }
    }
}