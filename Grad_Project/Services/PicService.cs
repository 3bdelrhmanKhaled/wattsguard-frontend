using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Grad_Project.Services
{
    public class PicService:IPicService
    {
        private readonly Cloudinary cloudinary;
        public PicService()
        {
            var account = new Account
            {
                Cloud = "",
                ApiKey = "",
                ApiSecret = ""
            };
            cloudinary = new Cloudinary(account);

        }
        public async Task<UploadResult> AddPicAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;

        }
        public async Task<DeletionResult>DeletePicAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await cloudinary.DestroyAsync(deleteParams);
            return result;

        }
    }
}
