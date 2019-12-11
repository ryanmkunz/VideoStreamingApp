using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using RyanTube.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace RyanTube.Controllers
{
    [CustomAuthorize]
    public class VideosController : BaseController
    {                
        public string bucketName = Keys.AwsBucketName;
        public static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        public static AmazonS3Client s3Client = new AmazonS3Client(Keys.AwsAccessKeyId, Keys.AwsSecretAccessKey);

        public ActionResult ListVideos()
        {
            var videos = GetVideosWithTagsAndComments();

            return View(videos);
        }

        public ActionResult ListVideosWithCurrentUserTag()
        {
            var videosWithTagsAndComments = GetVideosWithTagsAndComments();
            var userName = GetCurrentUser().UserName;
            List<Video> videosWithCurrentUserTag = new List<Video>();

            foreach (var video in videosWithTagsAndComments)
            {
                foreach (var tag in video.UserTags)
                {
                    if (tag.UserName == userName)
                    {
                        videosWithCurrentUserTag.Add(video);
                    }
                }
            }

            return View("ListVideos", videosWithCurrentUserTag);            
        }

        [CustomAdminAuthorize]
        public ActionResult AdminListVideos()
        {
            return View(GetVideos());
        }

        public IEnumerable<Video> GetVideos()
        {                                   
            S3DirectoryInfo dir = new S3DirectoryInfo(s3Client, bucketName, "GameCaptures");
            
            foreach (IS3FileSystemInfo file in dir.GetFileSystemInfos())
            {                
                var url = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = "GameCaptures/" + file.Name,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                var video = db.Videos.FirstOrDefault(v => v.FileName == file.Name);

                if (video != null)
                {
                    db.Videos.FirstOrDefault(v => v.FileName == file.Name).Url = url;                    
                    db.SaveChanges();                    
                }
                else
                {
                    Video newVideo = new Video
                    {
                        Url = url,
                        Title = file.Name,
                        FileName = file.Name,
                        LastUpdated = file.LastWriteTime
                    };

                    db.Videos.Add(newVideo);
                    db.SaveChanges();
                }                             
            }
            var videos = db.Videos.ToList();

            return videos;
        }    
        
        public Video GetVideoComments(Video video)
        {            
            var comments = db.Comments.Where(c => c.VideoId == video.Id).ToList();                        
            video.Comments = comments;

            return video;
        }

        public Video GetUserTags(Video video)
        {            
            var userTags = db.UserTags.Where(u => u.VideoId == video.Id).ToList();
            video.UserTags = userTags;

            return video;
        }

        public IEnumerable<Video> GetVideosWithTagsAndComments()
        {
            var videos = GetVideos();
            List<Video> videosWithTagsAndComments = new List<Video>();

            foreach (var item in videos)
            {
                var videoWithTags = GetUserTags(item);
                videosWithTagsAndComments.Add(GetVideoComments(videoWithTags));
            }

            return videosWithTagsAndComments;
        }

        public ActionResult Watch(int id)
        {
            var video = db.Videos.FirstOrDefault(v => v.Id == id);
            var videoWithComments = GetVideoComments(video);
            var videoWithTagsAndComments = GetUserTags(videoWithComments);

            return View(videoWithTagsAndComments);
        }        
        
        [HttpGet]
        public ActionResult Edit(string FileName)
        {
            List<SelectListItem> thumbnails = new List<SelectListItem>();
            var thumbnailFileNames = Directory.GetFiles(Server.MapPath("~/Images"));

            foreach (var item in thumbnailFileNames)
            {
                var fileName = Path.GetFileName(item);
                thumbnails.Add(new SelectListItem { Text = fileName, Value = "~/Images/" + fileName });
            }

            ViewBag.Thumbnails = thumbnails;
            var video = db.Videos.FirstOrDefault(v => v.FileName == FileName);

            if (video == null)
            {
                var newVideo = GetVideos().FirstOrDefault(v => v.FileName == FileName);

                return View(newVideo);
            }

            return View(video);
        }

        [HttpPost]
        public ActionResult Edit(Video video)
        {
            db.Videos.FirstOrDefault(v => v.Id == video.Id).Title = video.Title;
            db.Videos.FirstOrDefault(v => v.Id == video.Id).Description = video.Description;
            db.Videos.FirstOrDefault(v => v.Id == video.Id).Thumbnail = video.Thumbnail;
            db.SaveChanges();

            var currentUserRole = GetCurrentUser().UserRole;

            if (currentUserRole != "Admin")
            {
                return RedirectToAction("ListVideos");
            }

            return RedirectToAction("AdminListVideos");
        }               
    }
}