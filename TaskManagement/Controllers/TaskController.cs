using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTO.Task;
using TaskManagement.Services.Task;

namespace TaskManagement.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _taskService.GetAll();

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpGet("{id:int}", Name = "GetTask")]
        public async Task<IActionResult> GetAll(int id)
        {
            var res = await _taskService.GetById(id);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksForUser(string userId)
        {
            var res = await _taskService.GetTasksForUser(userId);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpPost("add")]
        [Authorize(Policy = "canCreateTask")]
        public async Task<IActionResult> Add([FromForm] CreateTaskRequestDTO request)
        {
            var res = await _taskService.Create(request);


            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpPut("Update")]
        [Authorize(Policy = "canUpdateTask")]
        public async Task<IActionResult> Update([FromForm] UpdateTaskRequestDTO request)
        {
            var res = await _taskService.Update(request);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "canDeleteTask")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _taskService.Remove(id);

            if(res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

    }
}
