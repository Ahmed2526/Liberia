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
        string SaveImages(IFormFile formFile);
        string UpdateImages(IFormFile formFile, string OldPhoto);
        //void DeleteImages(string path);
    }
}
