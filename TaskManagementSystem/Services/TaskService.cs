using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DBWork;
using TaskManagementSystem.DTO;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
	public class TaskService
	{
		private readonly ITaskRepository _taskRep;

		public TaskService(
			ITaskRepository taskRep)
		{
			_taskRep = taskRep;
		}

		public async Task<TMS.Task> FindAsync(Guid id)
			=> await _taskRep.FindAsync(id);

		public async Task<IEnumerable<GetTaskDTO>> GetAllAsync()
			=> await _taskRep.GetAllAsync();

		public async Task<TMS.Task> AddAsync(CreateTaskDTO task)
		{
			var newTask = await _taskRep.AddAsync(task);
			await UpdateParentTaskState(task.ParentTaskId ?? Guid.Empty);
			return newTask;
		}

		public async Task UpdateAsync(Guid id, UpdateTaskDTO task)
		{
			var taskEntity = await _taskRep.FindAsync(id);
			if (taskEntity != null)
			{
				await _taskRep.UpdateAsync(task, taskEntity);
				await UpdateParentTaskState(task.ParentTaskId ?? Guid.Empty);
			}
		}

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

		public bool IsTaskExists(Guid id)
			=> _taskRep.IsTaskExists(id);

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