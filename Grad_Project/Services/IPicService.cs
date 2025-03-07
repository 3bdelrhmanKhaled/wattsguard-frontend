using CloudinaryDotNet.Actions;

namespace Grad_Project.Services
{
    public interface IPicService
    {
        Task<UploadResult> AddPicAsync(IFormFile file);
        Task<DeletionResult> DeletePicAsync(string publicId);

    }
}
