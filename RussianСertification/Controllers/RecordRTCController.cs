using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        //[HttpPost]
        //public IActionResult PostRecordedAudioVideo()
        //{
        //    foreach (var upload in Request.Form.Files)
        //    {
        //        var path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
        //        var file = Request.Form.Files[upload.FileName];
        //        if (file == null) continue;

        //        file.SaveAs(Path.Combine(path, Request.Form[0]));

        //        StoreInFolder(file, filepath);
        //    }
        //    return Json(Request.Form[]);
        //}

        // ---/RecordRTC/DeleteFile
        [HttpPost]
        public IActionResult DeleteFile()
        {
            var fileUrl = AppDomain.CurrentDomain.BaseDirectory + "uploads/" + Request.Form["delete-file"] + ".webm";
            new FileInfo(fileUrl).Delete();
            return Json(true);
        }

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