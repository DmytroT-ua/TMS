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
	/// <summary>
	/// Contains all methods to handle task related requests
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly TaskService _taskService;

		public TaskController(TaskService taskService)
		{
			_taskService = taskService;
		}

		/// <summary>
		/// Gets all tasks.
		/// </summary>
		/// <returns>Returns all tasks.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<GetTaskDTO>>> GetTasks()
			=> await _taskService.GetAllAsync() as List<GetTaskDTO>;

		/// <summary>
		/// Gets single task by id.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns task.</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<TMS.TMSTask>> GetTask(Guid id)
		{
			var task = await _taskService.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}
			return task;
		}

		/// <summary>
		/// Updates task by id.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <param name="task">Update task model</param>
		/// <returns>Returns action result</returns>
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

		/// <summary>
		/// Creates new task.
		/// </summary>
		/// <param name="task">Create task model</param>
		/// <returns>Returns created task.</returns>
		[HttpPost]
		public async Task<ActionResult<Task>> PostTask(CreateTaskDTO task)
		{
			TMS.TMSTask crearedTask = await _taskService.AddAsync(task);
			return CreatedAtAction("GetTask", new { id = crearedTask.Id }, crearedTask);
		}

		/// <summary>
		/// Deletes task by id.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns action result.</returns>
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteTask(Guid id)
		{
			bool result = await _taskService.RemoveAsync(id);
			return result ? Ok() : Conflict() as ActionResult;
		}

		/// <summary>
		/// Generates report of tasks in progress for specific date.
		/// </summary>
		/// <param name="date">Date when task was in progress</param>
		/// <returns>Returns csv file</returns>
		[HttpGet]
		[Route("GetReport/{date}")]
		public async Task<FileResult> GetReport(DateTime date)
			=> File(await _taskService.GetReport(date), "csv/text", "Report.csv");
	}
}