using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ÖdevDağıtım.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("sadece-uyeler")]
        public IActionResult GetKullanici()
        {
            return Ok(new { message = "Başarılı! Eğer bunu görüyorsan geçerli bir Token'ın var demektir." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("sadece-admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { message = "Başarılı! Eğer bunu görüyorsan hem Token'ın var, hem de ADMIN rolüne sahipsin!" });
        }

        [AllowAnonymous]
        [HttpGet("herkese-acik")]
        public IActionResult GetPublic()
        {
            return Ok(new { message = "Buraya Token olmadan, dünyadaki herkes girebilir." });
        }
    }
}