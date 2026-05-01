using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFlowPro.Application.Common.Models;
using WorkFlowPro.Application.Features.WorkItems.DTOs;
using WorkFlowPro.Infrastructure.Services;

namespace WorkFlowPro.API.Controllers
{

    [ApiController]
    [Route("api/projects/{projectId}/workitems")]
    [Authorize]
    public class WorkItemController : ControllerBase
    {

        private readonly WorkItemService _workItemService;

        public WorkItemController(WorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        private Guid CurrentUserId() => Guid.Parse(User.FindFirst("userId")?.Value!);

        [HttpGet]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            var userId = CurrentUserId();
            var result = await _workItemService.GetByProjectAsync(projectId, userId);

            return Ok(ApiResponse<IEnumerable<WorkItemSummaryDto>>.SuccessResult(result));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid projectId, Guid id)
        {
            var userId = CurrentUserId();
            var result = await _workItemService.GetByIdAsync(id, userId);

            return Ok(
                ApiResponse<WorkItemResponseDto>
                    .SuccessResult(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateWorkItemDto dto)
        {
            var userId = CurrentUserId();
            var result = await  _workItemService.CreateAsync(projectId, dto, userId);

            return StatusCode(201,ApiResponse<WorkItemResponseDto>.SuccessResult(result, "Task Created Successfully", 201));
        }

        // PUT /api/projects/{projectId}/workitems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid projectId,
            Guid id,
            [FromBody] UpdateWorkItemDto dto)
        {
            var userId = CurrentUserId();
            var result = await _workItemService
                .UpdateAsync(id, dto, userId);

            return Ok(
                ApiResponse<WorkItemResponseDto>
                    .SuccessResult(
                        result,
                        "Task updated successfully"));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            Guid projectId,
            Guid id,
            [FromBody] UpdateWorkItemStatusDto dto)
        {
            var userId = CurrentUserId();
            var result = await _workItemService
                .UpdateStatusAsync(id, dto, userId);

            return Ok(
                ApiResponse<WorkItemResponseDto>
                    .SuccessResult(
                        result,
                        "Status updated"));
        }

        // DELETE /api/projects/{projectId}/workitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid projectId,
            Guid id)
        {
            var userId = CurrentUserId();
            await _workItemService.DeleteAsync(id, userId);

            return Ok(ApiResponse.SuccessResult(
                "Task deleted successfully"));
        }
    }
}
