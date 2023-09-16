using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Drawing;

namespace TravelProject1._0.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureApiController : ControllerBase
    {
        public PictureApiController() { }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto ([FromForm] IFormFile file) 
        {       
            if (file.Length > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filedata=Path.Combine("uploads",filename);
                if (file.ContentType == "Image/jpeg" && file.ContentType == "Image/png")
                {
                    using (var stream = new FileStream(filedata,FileMode.Create)) 
                    {
                      await file.CopyToAsync(stream);
                    }
                    return Ok();
                }
                return BadRequest();
            }

            return BadRequest();


        }
    }
}
