namespace Instaq.API.Debug.Controllers
{
    using System;
    using System.IO;
    using Instaq.Contract;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFileHandler fileHandler;

        public UserController(IFileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
        }

        [HttpGet("Img/{fileName}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetUserImage(string fileName)
        {
            if (fileName.Contains(".."))
            {
                return this.BadRequest();
            }
            try
            {
                var image = this.fileHandler.GetFile(FileType.User, fileName);
                return this.File(image, "image/jpeg");
            }
            catch (FileNotFoundException)
            {
                return this.NotFound("Image not found");
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }
    }
}
