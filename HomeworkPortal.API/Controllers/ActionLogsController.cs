using HomeworkPortal.API.DTOs;
using HomeworkPortal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActionLogsController : ControllerBase
    {
        private readonly IActionLogService _actionLogService;

        public ActionLogsController(IActionLogService actionLogService)
        {
            _actionLogService = actionLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] PaginationParams paginationParams)
        {
            var result = await _actionLogService.GetLogsAsync(paginationParams);
            return Ok(result);
        }
    }
}