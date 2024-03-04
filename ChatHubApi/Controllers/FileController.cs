using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> File(IEnumerable<IFormFile> files)
        {
            try
            {
                var image = Request.Form.Files[0];
                UploadResult uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = image.FileName;
                uploadResult.FileName = untrustedFileName;

                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

              

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                return Ok(new GenericResponse<UploadResult> { Success = true, Data = uploadResult });

            }
            catch(Exception ex)
            {
                return Ok(new GenericResponse<UploadResult> { Success = false, Error = ex.Message });
            }



        }
    }
}
