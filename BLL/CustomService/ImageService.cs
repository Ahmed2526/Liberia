using BLL.ICustomService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.CustomService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private List<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int MaxAllowedFileSize = 2145728;

        public string SaveImages(IFormFile ImgFile)
        {
            try
            {
                if (ImgFile is not null)
                {
                    var ImgFileExtension = AllowedExtensions.Contains(Path.GetExtension(ImgFile.FileName));

                    //Check Extension
                    if (!ImgFileExtension)
                        return null;

                    //Check File Size
                    if (ImgFile.Length > MaxAllowedFileSize)
                        return null;

                    //New Names
                    var ImgFileNewName = Guid.NewGuid() + Path.GetExtension(ImgFile.FileName);

                    //Generating Paths
                    var ImgFilePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", ImgFileNewName);

                    //Physical Copying
                    using var stream = System.IO.File.Create(ImgFilePath);
                    ImgFile.CopyTo(stream);


                    return ImgFileNewName;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateImages(IFormFile formFile, string OldPhotoName)
        {
            try
            {
                if (formFile is not null)
                {
                    var idPhotoExtension = AllowedExtensions.Contains(Path.GetExtension(formFile.FileName));

                    //Check Extension
                    if (!idPhotoExtension)
                        return null;

                    //Check File Size
                    if (formFile.Length > MaxAllowedFileSize)
                        return null;

                    //New Names
                    var PhotoNewName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);

                    //Generating Paths
                    var OldPhotoPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", OldPhotoName);
                    var NewPhotoPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", PhotoNewName);

                    //Physical Copying
                    using var IdPhotostream = System.IO.File.Create(NewPhotoPath);
                    formFile.CopyTo(IdPhotostream);

                    //DeleteOldPhoto 
                    if (OldPhotoPath is not null)
                        System.IO.File.Delete(OldPhotoPath);

                    return PhotoNewName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
       
    }
}
