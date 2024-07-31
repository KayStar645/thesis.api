using Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.WebApi.Controllers.S3
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class AmazonS3Controller : ControllerBase
    {
        private readonly IAmazonS3Service _AmazonS3Service;

        public AmazonS3Controller(IAmazonS3Service pAmazonS3Service)
        {
            _AmazonS3Service = pAmazonS3Service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileAsync(string pKey)
        {
            try
            {
                var publicUrl = await _AmazonS3Service.GetFileCidFromS3Async(pKey);

                return Ok(new { url = publicUrl });
            }    
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadFileAsync(string pPath, IFormFile pFile)
        {
            if (pFile == null || pFile.Length == 0)
                return BadRequest("No file uploaded.");

            var publicUrl = await _AmazonS3Service.UploadFileAsync(pPath, pFile);

            if (publicUrl == null)
                return StatusCode(500, "An error occurred while uploading the file.");

            return Ok(new { url = publicUrl });
        }
    }
}
