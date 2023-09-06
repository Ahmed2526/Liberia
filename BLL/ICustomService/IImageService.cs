using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ICustomService
{
    public interface IImageService
    {
        Dictionary<string, string> SaveImages(IFormFile formFile);
        Dictionary<string, string> UpdateImages(IFormFile formFile, string OldPhoto, string OldThumb);
        //void DeleteImages(string path);
    }
}
