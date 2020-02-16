using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly TaskService _taskService;

		public TaskController(TaskService taskService)
		{
			_taskService = taskService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<GetTaskDTO>>> GetTasks()
			=> await _taskService.GetAllAsync() as List<GetTaskDTO>;

		[HttpGet("{id}")]
		public async Task<ActionResult<TMS.Task>> GetTask(Guid id)
		{
			var task = await _taskService.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}
			return task;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTask(Guid id, UpdateTaskDTO task)
		{
			if (id != task.Id)
			{
				return BadRequest();
			}

			try
			{
				await _taskService.UpdateAsync(id, task);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!_taskService.IsTaskExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Task>> PostTask(CreateTaskDTO task)
		{
			TMS.Task crearedTask = await _taskService.AddAsync(task);
			return CreatedAtAction("GetTask", new { id = crearedTask.Id }, crearedTask);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteTask(Guid id)
		{
			bool result = await _taskService.RemoveAsync(id);
			return result ? Ok() : Conflict() as ActionResult;
		}

		[HttpGet]
		[Route("GetReport/{date}")]
		public async Task<FileResult> GetReport(DateTime date)
			=> File(await _taskService.GetReport(date), "csv/text", "Report.csv");
	}
}