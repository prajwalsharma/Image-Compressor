using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;

namespace Image_Compressor.Controllers
{

    [ApiController]
    public class FilesController : ControllerBase
    {
        // Compress File
        [HttpPost("api/upload")]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {

            byte[] result = null;

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                stream.Position = 0;
                ImageOptimizer optimizer = new ImageOptimizer();
                optimizer.Compress(stream);
                result = stream.ToArray();
            }

            return File(result, formFile.ContentType);

        }

        // Resize File
        [HttpPost("api/resize")]
        public async Task<IActionResult> ResizeFile(IFormFile formFile)
        {

            byte[] result = null;

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                stream.Position = 0;

                using (var image = new MagickImage(stream))
                {
                    var size = new MagickGeometry(400, 200);
                    size.IgnoreAspectRatio = true;
                    image.Resize(size);
                    var ms = new MemoryStream();
                    image.Quality = 100;
                    image.Write(ms);
                    result = ms.ToArray();
                }

            }

            return File(result, formFile.ContentType);

        }
    }
}
