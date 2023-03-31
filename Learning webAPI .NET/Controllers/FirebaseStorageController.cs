using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Learning_webAPI_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly StorageClient storageClient;
        private readonly string bucketName = "shuusan-tutorial.appspot.com"; // Replace with your bucket name
        private readonly string imageFolder = "ASP.NET Tutorial/images/";
        public StorageController()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "credentials.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            storageClient = StorageClient.Create();
        }

        // GET: api/Storage/filename
        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string filename)
        {
            var storageObject = await storageClient.GetObjectAsync(bucketName, imageFolder+ filename);
            var memoryStream = new MemoryStream();
            await storageClient.DownloadObjectAsync(bucketName, imageFolder + filename, memoryStream);
            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, storageObject.ContentType);
        }

        // POST: api/Storage
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var storageObject = await storageClient.UploadObjectAsync(
                    bucketName,
                    fileName,
                    null,
                    memoryStream);
            }
            return Ok();
        }

        // PUT: api/Storage/filename
        [HttpPut("{filename}")]
        public async Task<IActionResult> Put(string filename, IFormFile file)
        {
            await storageClient.DeleteObjectAsync(bucketName, filename);
            var fileName = Path.GetFileName(file.FileName);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var storageObject = await storageClient.UploadObjectAsync(
                    bucketName,
                    fileName,
                    null,
                    memoryStream);
            }
            return Ok();
        }

        // DELETE: api/Storage/filename
        [HttpDelete("{filename}")]
        public async Task<IActionResult> Delete(string filename)
        {
            await storageClient.DeleteObjectAsync(bucketName, filename);
            return Ok();
        }
    }
}
