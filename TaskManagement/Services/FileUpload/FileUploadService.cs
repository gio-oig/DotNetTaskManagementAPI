namespace TaskManagement.Services.FileUpload
{

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>> UploadFilesAsync(List<IFormFile?> files)
        {
            List<string> uploadedPaths = new List<string>();


            foreach (var file in files)
            {
                if (file?.Length == 0)
                {
                    continue;
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fileLocation = Path.Combine("Files", uniqueFileName);
                string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, fileLocation);

                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedPaths.Add(fileLocation);
            }

            return uploadedPaths;
        }

        public void RemoveFile(string fileUrl)
        {
            string filePathToDelete = Path.Combine(_webHostEnvironment.ContentRootPath, fileUrl);
            if (File.Exists(filePathToDelete))
            {
                File.Delete(filePathToDelete);
            }
        }
    }
}
