﻿using BLL.ICustomService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

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
        //Max Photo Size is 2MB
        private int MaxAllowedFileSize = 2145728;

        public Dictionary<string, string> SaveImages(IFormFile ImgFile)
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
                    var thumbNailName = "thumb-" + Guid.NewGuid() + Path.GetExtension(ImgFile.FileName);

                    //Generating Paths
                    var ImgFilePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", ImgFileNewName);
                    var thumbNailPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/Thumbnails", thumbNailName);

                    //Physical Copying
                    using var stream = System.IO.File.Create(ImgFilePath);
                    ImgFile.CopyTo(stream);
                    stream.Dispose();

                    //Thumbnail Handeling
                    using var image = Image.Load(ImgFile.OpenReadStream());
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height));
                    image.Save(thumbNailPath);

                    //Return the image and its thumbnail.
                    var dictonary = new Dictionary<string, string>();
                    dictonary.Add("OriginalImg", ImgFileNewName);
                    dictonary.Add("Thumbnail", thumbNailName);

                    return dictonary;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, string> UpdateImages(IFormFile formFile, string OldPhotoName, string Oldthumbnail)
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
                    var ThumbNailNewName = "thumb-" + Guid.NewGuid() + Path.GetExtension(formFile.FileName);

                    //Generating Paths
                    var OldPhotoPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", OldPhotoName);
                    var NewPhotoPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", PhotoNewName);

                    var OldThumbNailPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/Thumbnails", Oldthumbnail);
                    var NewThumbNailPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/Thumbnails", ThumbNailNewName);

                    //DeleteOldPhoto 
                    if (!string.IsNullOrEmpty(OldPhotoName))
                        System.IO.File.Delete(OldPhotoPath);

                    if (!string.IsNullOrEmpty(Oldthumbnail))
                        System.IO.File.Delete(OldThumbNailPath);


                    //Physical Copying
                    using var IdPhotostream = System.IO.File.Create(NewPhotoPath);
                    formFile.CopyTo(IdPhotostream);

                    //Thumbnail Handeling
                    using var image = Image.Load(formFile.OpenReadStream());
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height));
                    image.Save(NewThumbNailPath);

                    //Return the image and its thumbnail.
                    var dictonary = new Dictionary<string, string>();
                    dictonary.Add("OriginalImg", PhotoNewName);
                    dictonary.Add("Thumbnail", ThumbNailNewName);

                    return dictonary;
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
