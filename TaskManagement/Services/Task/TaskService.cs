using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.Task;
using TaskManagement.Repository.IRepository;
using TaskManagement.Services.FileUpload;

namespace TaskManagement.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITaskRepository _taskRepository;
        private readonly IFileUploadService _uploadService;


        public TaskService(IWebHostEnvironment webHostEnvironment, ITaskRepository taskRepository, IFileUploadService uploadService)
        {
            _webHostEnvironment = webHostEnvironment;
            _taskRepository = taskRepository;
            _uploadService = uploadService;
        }

        public async Task<ServiceResponse<List<TaskModel>>> GetAll()
        {
            ServiceResponse<List<TaskModel>> _response = new();
            try
            {
                var tasks = await _taskRepository.GetAllAsync((a) => a.Files);
                _response.Data = tasks;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Could not get tasks";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ServiceResponse<TaskModel>> GetById(int taskId)
        {
            ServiceResponse<TaskModel> _response = new();

            try
            {
                var task = await _taskRepository.GetAsync(task => task.Files, task => task.Id == taskId);

                if (task is null)
                {
                    throw new Exception("could not find task");

                }

                _response.Data = task;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Could not get task";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ServiceResponse<List<TaskModel>>> GetTasksForUser(string userId)
        {
            ServiceResponse<List<TaskModel>> _response = new();

            try
            {
                var tasks = await _taskRepository.GetAllAsync(t => t.Files, t => t.AssignedUserId == userId);

                _response.Data = tasks;
            } catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "An error occurred while fetching tasks.";
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ServiceResponse<string>> Create(CreateTaskRequestDTO dto)
        {

            ServiceResponse<string> _response = new();

            TaskModel taskModel = new TaskModel
            {
                Title = dto.Title,
                Description = dto.Description,
                AssignedUserId = dto.AssignedUserId,
            };



            List<string> uploadedPaths = await _uploadService.UploadFilesAsync(dto.Files);

            taskModel.Files = new List<TaskFile>();
            foreach (var path in uploadedPaths)
            {
                var file = new TaskFile { URL = path };

                taskModel.Files.Add(file);

            }

            await _taskRepository.CreateAsync(taskModel);

            _response.Data = "Task Added Succesfully";

            return _response;
        }

        public async Task<ServiceResponse<TaskModel>> Update(UpdateTaskRequestDTO updateDto)
        {
            ServiceResponse<TaskModel> _response = new();
            try
            {
                var task = await _taskRepository.GetAsync(task => task.Files, task => task.Id == updateDto.Id);

                task.Title = updateDto.Title;
                task.Description = updateDto.Description;
                task.AssignedUserId = updateDto.AssignedUserId;

                List<int> fileIds = new();
                List<IFormFile?> newFiles = new();

                if (updateDto.Files is not null)
                {
                    fileIds = updateDto.Files.Where(f => f.Id.HasValue).Select(f => f.Id.Value).ToList();
                    newFiles = updateDto.Files.Where(f => f.Id == null).Select(f => f.File).ToList();
                }

                List<TaskFile> filesToRemove = task.Files.Where(f => !fileIds.Contains(f.Id)).ToList();
                foreach (var fileToRemove in filesToRemove)
                {
                    task.Files.Remove(fileToRemove);
                    string filePathToDelete = Path.Combine(_webHostEnvironment.ContentRootPath, fileToRemove.URL);
                    if (File.Exists(filePathToDelete))
                    {
                        File.Delete(filePathToDelete);
                    }
                }


                List<string> uploadedPaths = await _uploadService.UploadFilesAsync(newFiles);


                foreach (var path in uploadedPaths)
                {
                    var file = new TaskFile { URL = path };

                    task.Files.Add(file);

                }

                var updatedTask = await _taskRepository.UpdateAsync(task);

                _response.Data = updatedTask;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Could not update task";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

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

        public async Task<ServiceResponse<string>> Remove(int taskId)
        {
            ServiceResponse<string> _response = new();

            try
            {
                var task = await _taskRepository.GetAsync(t => t.Files, t => t.Id == taskId);

                if (task is null)
                {
                    _response.Success = false;
                    _response.Message = "Task not found.";
                    return _response;
                }

                foreach (var file in task.Files)
                {
                    string FilePathToDelete = Path.Combine(_webHostEnvironment.ContentRootPath, file.URL);

                    if(File.Exists(FilePathToDelete))
                    {
                        File.Delete(FilePathToDelete);
                    }
                }

                await _taskRepository.RemoveAsync(task);

                _response.Data = "Task and associated files deleted successfully.";
            } catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Could not delete task.";
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
