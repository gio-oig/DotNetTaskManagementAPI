namespace TaskManagement.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<List<string>> UploadFilesAsync(List<IFormFile?> files);
        void RemoveFile(string fileUrl);
    }
}
