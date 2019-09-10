namespace AutoTagger.API.Controllers
{
    using System;
    using System.IO;
    using AutoTagger.Contract;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IFileHandler fileHandler;

        public UserController(
            IFileHandler fileHandler
            )
        {
            //this.evaluationStorage = evaluationStorage;
            //this.logStorage = logStorage;
            this.fileHandler = fileHandler;
        }

        [Route("Img/{fileName}")]
        [HttpGet]
        public IActionResult GetUserImage(string fileName)
        {
            if (fileName.Contains(".."))
            {
                return this.StatusCode(500);
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
                return this.StatusCode(500);
            }
        }
    }
}
