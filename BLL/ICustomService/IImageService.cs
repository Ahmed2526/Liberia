using DAL.DTO;
using Microsoft.AspNetCore.Http;

namespace BLL.ICustomService
{
	public interface IImageService
	{
		Dictionary<string, string> SaveImages(IFormFile formFile);
		CommonResponse SaveProfileImage(IFormFile formFile, string id);
		Dictionary<string, string> UpdateImagesV2(IFormFile formFile, string OldPhoto, string OldThumb);


		//For Old Photos
		Dictionary<string, string> UpdateImages(IFormFile formFile, string OldPhoto, string OldThumb);
	}
}
