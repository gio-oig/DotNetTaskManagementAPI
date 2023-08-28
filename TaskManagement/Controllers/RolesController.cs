using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models.DTO.Role;
using TaskManagement.Services.Role;

namespace TaskManagement.Controllers
{
    [Route("roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDTO roleDto)
        {
            var resp = await _roleService.Create(roleDto.roleName);

            if(resp.Success)
            {
                return Ok(resp);
            } 

            return BadRequest(resp);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _roleService.GetAll();

            if(res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpPost("assignClaim")]
        public async Task<IActionResult> AssignClaim([FromBody] AssignClaimRequest req)
        {
            var res = await _roleService.AssignClaim(req);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }
    }
}
