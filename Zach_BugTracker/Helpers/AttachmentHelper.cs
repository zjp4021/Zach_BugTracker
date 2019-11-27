using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Helpers
{
    public class AttachmentHelper
    {
        public static string GetIcon(string fileName)
        {
            var imgPath = "";
            var ext = Path.GetExtension(fileName);
            switch (ext)
            {
                case ".pdf":
                    imgPath = "/Images/pdf-icon.png";
                    break;
                case ".doc":
                    imgPath = "/Images/doc-icon.png";
                    break;
                case ".docx":
                    imgPath = "/Images/docx-icon.png";
                    break;
                case ".xls":
                    imgPath = "/Images/xls-icon.png";
                    break;
                case ".xlsx":
                    imgPath = "/Images/xlsx-icon.png";
                    break;
                case ".txt":
                    imgPath = "/Images/txt-icon.png";
                    break;
                case ".zip":
                case ".rar":
                case ".7z":
                    imgPath = "/Images/zip-icon.png";
                    break;
                case ".xml":
                    imgPath = "/Images/xml-icon.jfif";
                    break;
                case ".jpg":
                case ".gif":
                case ".png":
                case ".jfif":
                    imgPath = fileName;
                    break;
                default:
                    imgPath = "/Images/Blank-icon.png";
                    break;
            }
            return imgPath;
        }
    }
}