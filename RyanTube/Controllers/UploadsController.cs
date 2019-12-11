using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    [CustomAuthorize]
    public class UploadsController : VideosController
    {
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    if (file != null)
                    {                        
                        string path = Path.Combine(Server.MapPath("~/VideoUploads"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);                        
                        var uploadStatus = await UploadFileAsync(path, file.FileName);                        

                        if (uploadStatus == "succeeded")
                        {                            
                            return RedirectToAction("Edit", "Videos", new { fileName = file.FileName });
                        }
                        else
                        {
                            ViewBag.FileStatus = uploadStatus;
                        }
                        DeleteFile(path);
                    }                    
                }
                catch (Exception e)
                {
                    ViewBag.FileStatus = e;
                }

            }
            return View("Upload");
        }            

        public async Task<string> UploadFileAsync(string filePath, string fileName)
        {
            string uploadStatus;

            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                using (var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload,
                        bucketName + "/GameCaptures", fileName).ConfigureAwait(false);
                }

                uploadStatus = "succeeded";
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                uploadStatus = "Error encountered on server. Message:'{0}' when writing an object" + e.Message.ToString();

                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                uploadStatus = "Unknown encountered on server. Message:'{0}' when writing an object" + e.Message.ToString();

                throw;
            }

            return uploadStatus;
        }        

        public void DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attributes = System.IO.File.GetAttributes(path);

                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes &= ~FileAttributes.ReadOnly;
                    System.IO.File.SetAttributes(path, attributes);
                    System.IO.File.Delete(path);
                }
                else
                {                    
                    throw;
                }
            }
        }
    }
}