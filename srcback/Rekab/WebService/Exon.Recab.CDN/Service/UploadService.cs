using System;
using Exon.Recab.Domain.SqlServer;
using System.Web;
using System.IO;
using System.Web.Configuration;
using Exon.Recab.Infrastructure.Utility.Security;
using System.Drawing;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Domain.Constant.CS.Exception;
using System.Drawing.Imaging;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace Exon.Recab.CDN.Service
{
    public class UploadService
    {
        private readonly SdbContext _sdb;

        public UploadService()
        {
            _sdb = new SdbContext();
        }

        public bool VerifyImage(Stream imageStream)
        {
            //if (imageStream.Length > 0)
            //{
            //    byte[] header = new byte[4]; // Change size if needed.
            //    string[] imageHeaders = new[]{
            //    "\xFF\xD8", // JPEG
            //    "BM",       // BMP
            //    "GIF",      // GIF
            //    Encoding.ASCII.GetString(new byte[]{137, 80, 78, 71})}; // PNG

            //    imageStream.Read(header, 0, header.Length);

            //    bool isImageHeader = imageHeaders.Count(str => Encoding.ASCII.GetString(header).StartsWith(str)) > 0;
            //    if (isImageHeader == true)
            //    {
            //        try
            //        {
            //            System.Drawing.Image.FromStream(imageStream).Dispose();
            //            imageStream.Close();
            //            return true;
            //        }

            //        catch
            //        {

            //        }
            //    }
            //}

            //imageStream.Close();
            //return false;

            return true;
        }

        public string SaveBase64(string data, long userId, string ex, string basePath ,bool size ,bool waterMark)
        {
            Domain.Entity.User user= _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            string watermarkText = "Copyright @ Rekab.ir";

            byte[] bytes = Convert.FromBase64String(data);            

            string name = CodeHelper.NewKey();

            string extenion = "." + ex;

            string directory = CodeHelper.NewKey(user.Id);

            string path = basePath + @"\" + directory;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string savedFileName = Path.Combine(path, name + extenion);
            if (!size)
            {
              bool res=this.ResizeAndWaterMark(image: bytes,
                                        watermarkText: watermarkText,
                                        saveFilePath: Path.Combine(path, name  + extenion),
                                        isize : false,
                                        waterMark : waterMark);

                if (!res)
                    return "-1";

                return WebConfigurationManager.AppSettings["BaseUrl"] +
                       WebConfigurationManager.AppSettings["RecabUpload"] +
                       "/" + directory + "/"
                       + name + extenion;
            }


          bool result=this.ResizeAndWaterMark(image: bytes,                                  
                                   newWith: 200,
                                   watermarkText: watermarkText,
                                   saveFilePath: Path.Combine(path, name + "X200" + extenion));

            if (!result)
                return "-1";


            this.ResizeAndWaterMark(image: bytes,                                 
                                   newWith: 500,
                                   watermarkText: watermarkText,
                                   saveFilePath: Path.Combine(path, name + "X500" + extenion));

            this.ResizeAndWaterMark(image: bytes,
                                   newWith: 750,
                                   watermarkText: watermarkText,
                                   saveFilePath: Path.Combine(path, name + "X750" + extenion));

            this.ResizeAndWaterMark(image: bytes,
                                   newWith: 1000,
                                   watermarkText: watermarkText,
                                   saveFilePath: Path.Combine(path, name + "X1000" + extenion));

            GC.Collect();

            return WebConfigurationManager.AppSettings["BaseUrl"] + WebConfigurationManager.AppSettings["RecabUpload"] + "/" + directory + "/" + name+"X200" + extenion;

        }
        
        public string SavePostedFile(HttpPostedFile data, long id, string path)
        {
            string ex = Path.GetExtension(data.FileName);

            string name = Guid.NewGuid().ToString() + ex;

            var user = _sdb.Users.Find(id);

            if (!Directory.Exists(path + @"\" + user.Email.ToString()))
            { Directory.CreateDirectory(path + @"\" + user.Email.ToString()); }

            string savedFileName = Path.Combine(path + @"\" + user.Email.ToString(), Path.GetFileName(name));
            data.SaveAs(savedFileName);

            return WebConfigurationManager.AppSettings["reselerupload"] + "/" + user.Email.ToString() + "/" + name;

        }

        private bool ResizeAndWaterMark(byte[] image,
                                        string watermarkText , 
                                        string saveFilePath, 
                                        int newWith = 0 , 
                                        bool isize =true ,
                                        bool waterMark =false)
        {   
            using (var inMS = new MemoryStream(image))
            {
                using (var osMS = new MemoryStream())
                {                    
                    using (var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        Image inputImage = imageFactory.Load(inMS).Image;

                        if (isize)
                        {
                            if (inputImage.Size.Height < 300 || inputImage.Size.Width < 500)
                                return false;
                        }

                        for (int i = 0; i < newWith /200; i++)
                        {
                            watermarkText = watermarkText + " " + watermarkText;
                        }

                        double ratio = (double)inputImage.Height / (double)inputImage.Width;

                        int newHeight = (int)((newWith != 0 ? newWith : inputImage.Width) * ratio);

                        int quality = 100;

                        var size = new Size(width: newWith, height: newHeight);

                        imageFactory.Load(inMS)                                                     
                                    .Resize(size)
                                    .Quality(quality)
                                    .Watermark(new TextLayer()
                                    {
                                        Text =waterMark ? "":watermarkText ,
                                        Style = FontStyle.Italic,                                      
                                        FontColor = Color.FromArgb(185,191,191),
                                        Opacity = 100,                                
                                        FontFamily = FontFamily.GenericSansSerif,
                                        FontSize = 16
                                    })
                                    .Save(osMS);

                        FileStream fs = File.OpenWrite(saveFilePath);
                        osMS.WriteTo(fs);
                        fs.Flush();
                        fs.Close();
                        osMS.Close();
                    }
                }
            }

            return true; 

        
        }      

    }
}
