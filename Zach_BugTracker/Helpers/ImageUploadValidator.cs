using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Zach_BugTracker.Helpers
{
    public static class ImageUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null) return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024) return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                            ImageFormat.Png.Equals(img.RawFormat) ||
                            ImageFormat.Tiff.Equals(img.RawFormat) ||
                            ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }
        }
    }


    public static class FileUploadValidator
    {
        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null) return false;

            var maxSize = WebConfigurationManager.AppSettings["MaxFileSize"];
            var minSize = WebConfigurationManager.AppSettings["MinFileSize"];


            if (file.ContentLength > Convert.ToInt32(maxSize) || file.ContentLength < Convert.ToInt32(minSize))

                return false;

            try
            {
                var allowedExtensions = WebConfigurationManager.AppSettings["AllowedAttachmentExtensions"];
                var fileExt = Path.GetExtension(file.FileName);
                return allowedExtensions.Contains(fileExt);

            }
            catch { return false; }
        }
    }


}