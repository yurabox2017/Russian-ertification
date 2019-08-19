using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RussianСertification.Controllers
{
    public class RecordRTCController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private IHostingEnvironment _hostingEnvironment;

        public RecordRTCController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        //[HttpPost]
        //public IActionResult PostRecordedAudioVideo(IList<IFormFile> formFile)
        //{
        //    foreach (IFormFile source in formFile)
        //    { }
        //    return this.View();
        //}

        [HttpPost]
        public async Task<IActionResult> PostRecordedAudioVideo([FromForm] IList<IFormFile> formFile)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            foreach (var file in formFile)
            {
                if (file.Length > 0)
                {
                    string filename = ContentDispositionHeaderValue.Parse(formFile[0].ContentDisposition).FileName.Trim('"');
                    var filePath = Path.Combine(uploads, file.FileName) + ".webm";
                    //using (FileStream fs = System.IO.File.Create(filePath))
                    //{
                    //    file.CopyTo(fs);
                    //    fs.Flush();
                    //}
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //  file.CopyTo(fileStream);
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return View();
            // return Json(Request.Form["0"]);
        }

        // ---/RecordRTC/DeleteFile
        //[HttpPost]
        //public IActionResult DeleteFile()
        //{
        //    var fileUrl = AppDomain.CurrentDomain.BaseDirectory + "uploads/" + Request.Form["delete-file"] + ".webm";
        //    new FileInfo(fileUrl).Delete();
        //    return Json(true);
        //}

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}