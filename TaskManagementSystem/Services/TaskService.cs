using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DBWork;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
	/// <summary>
	/// Contains all methods for performing basic task functions.
	/// </summary>
	public class TaskService
	{
		private readonly ITaskRepository _taskRep;

		public TaskService(
			ITaskRepository taskRep)
		{
			_taskRep = taskRep;
		}

		/// <summary>
		/// Gets single task by id.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns task.</returns>
		public async Task<TMSTask> FindAsync(Guid id)
			=> await _taskRep.FindAsync(id);

		/// <summary>
		/// Gets all tasks.
		/// </summary>
		/// <returns>Returns all tasks.</returns>
		public async Task<IEnumerable<GetTaskDTO>> GetAllAsync()
			=> await _taskRep.GetAllAsync();

		/// <summary>
		/// Creates new task.
		/// </summary>
		/// <param name="task">Create task model</param>
		/// <returns>Returns created task.</returns>
		public async Task<TMSTask> AddAsync(CreateTaskDTO task)
		{
			var newTask = await _taskRep.AddAsync(task);
			await UpdateParentTaskState(task.ParentTaskId ?? Guid.Empty);
			return newTask;
		}

		/// <summary>
		/// Updates task by id.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <param name="task">Update task model</param>
		public async Task UpdateAsync(Guid id, UpdateTaskDTO task)
		{
			var taskEntity = await _taskRep.FindAsync(id);
			if (taskEntity != null)
			{
				await _taskRep.UpdateAsync(task, taskEntity);
				await UpdateParentTaskState(task.ParentTaskId ?? Guid.Empty);
			}
		}

		/// <summary>
		/// Removes task by id if children not exists.
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns true if task dont have children and was removed.</returns>
		public async Task<bool> RemoveAsync(Guid id)
		{
			bool result = default;
			var task = await _taskRep.GetTaskWithChildren(id);
			if (task.Children.Count == 0)
			{
				await _taskRep.RemoveAsync(task);
				result = true;
			}
			return result;
		}

		/// <summary>
		/// Generates bytes for report of tasks in progress for specific date.
		/// </summary>
		/// <param name="date">Date when task was in progress</param>
		/// <returns>Returns bytes for csv report.</returns>
		public async Task<byte[]> GetReport(DateTime date)
		{
			IEnumerable<ReportTaskDTO> reportData = await _taskRep.GetTasksForReport(date);

			var taskCsv = new StringBuilder();
			GetReportRows(reportData).ToList().ForEach(line =>
			{
				taskCsv.AppendLine(string.Join(",", line));
			});

			byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", GetReportHeaders())}\r\n{taskCsv.ToString()}");
			return buffer;
		}

		/// <summary>
		/// Check if task exists.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns is task exists</returns>
		public bool IsTaskExists(Guid id)
			=> _taskRep.IsTaskExists(id);

		/// <summary>
		/// Recursively updates parent tasks state.
		/// </summary>
		/// <param name="taskId">Child task id.</param>
		private async Task UpdateParentTaskState(Guid taskId)
		{
			if (!Guid.Empty.Equals(taskId) && await _taskRep.GetChildrenAmount(taskId) != 0)
			{
				Guid resultState;
				int inProgressAmount = await _taskRep.GetChildrenAmountByState(taskId, CONST.Task.State.InProgress);
				if (inProgressAmount == 0)
				{
					int plannedAmount = await _taskRep.GetChildrenAmountByState(taskId, CONST.Task.State.Planned);
					resultState = plannedAmount == 0 ?
						CONST.Task.State.Completed : CONST.Task.State.Planned;
				}
				else
				{
					resultState = CONST.Task.State.InProgress;
				}
				await _taskRep.UpdateTaskState(taskId, resultState);

				await UpdateParentTaskState(await _taskRep.GetParentTaskId(taskId));
			}
		}

		/// <summary>
		/// Concatenate task report columns in to use in csv format
		/// </summary>
		/// <param name="reportData"></param>
		/// <returns>Returns columns for task report</returns>
		protected virtual IEnumerable<object> GetReportRows(IEnumerable<ReportTaskDTO> reportData)
			=> reportData.Select(task => new object[]
			{
				$"{task.Name}",
				$"{task.Description}",
				$"{task.State}",
				$"{task.Parent}",
				$"{task.StartDate}",
				$"{task.StartDate}"
			});

		/// <summary>
		/// Creates array of columns for task report
		/// </summary>
		/// <returns>Returns task report columns headers</returns>
		protected virtual string[] GetReportHeaders()
			=> new string[]
			{
				"Task Name",
				"Description",
				"State",
				"Start Date",
				"End Date",
				"Parent Task"
			};
	}
}