using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace task2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly string _filesFolder;

        public FilesController(IConfiguration config, IHostingEnvironment hostingEnvironment)
        {
            _filesFolder = Path.Combine(hostingEnvironment.WebRootPath, config["FilesFolder"]);
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            var contents = Directory.GetFiles(_filesFolder);
            var result = contents.Select(x => Path.GetFileName(x));
            return result;
        }

        // GET api/values/5
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {

            if (string.IsNullOrEmpty(filename))
            {
                return HttpBadRequest("Empty filename");
            }

            var fullName = Path.Combine(_filesFolder, Path.GetFileName(filename));
            if (System.IO.File.Exists(fullName))
            {
                return new FileStreamResult(new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/octet-stream");
            }
            
            return HttpNotFound("No such file");
        }

        // POST api/values
        [HttpPost(Name="create")]
        public async Task<IActionResult> Create(ICollection<IFormFile> files)
        {
            if (files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var newFilename = Path.Combine(_filesFolder, fileName);
                        if (System.IO.File.Exists(newFilename) == false)
                        {
                            using (var fStream = new FileStream(newFilename, FileMode.CreateNew))
                            {
                                fStream.Position = 0;
                                var inStream = file.OpenReadStream();
                                inStream.Position = 0;
                                inStream.CopyToAsync(fStream).Wait();
                            }
                        }
                    }
                }
                return Ok();
            }
            return HttpBadRequest("No file attached");
        }

        // PUT api/values/5
        [HttpPut]
        public void Put()
        {
            throw new NotSupportedException();
        }

        // DELETE api/values/5
        [HttpDelete("{filename}")]
        public IActionResult Delete(string filename)
        {
            if (string.IsNullOrEmpty(filename) == false)
            {
                var fullName = Path.Combine(_filesFolder, Path.GetFileName(filename));
                if (System.IO.File.Exists(fullName))
                {
                    System.IO.File.Delete(fullName);
                    return Ok();
                }
                return HttpNotFound("No such file");
            }
            return HttpBadRequest("Empty filename");
        }
    }
}
