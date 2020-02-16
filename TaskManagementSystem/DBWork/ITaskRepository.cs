using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.DTO;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	public interface ITaskRepository
	{
		Task<TMS.Task> FindAsync(Guid id);

		Task<IEnumerable<GetTaskDTO>> GetAllAsync();

		Task<TMS.Task> AddAsync(CreateTaskDTO task);

		Task UpdateAsync(UpdateTaskDTO task, TMS.Task taskEntity);

		Task<TMS.Task> RemoveAsync(TMS.Task task);

		Task<Guid> GetParentTaskId(Guid taskId);

		Task<Guid> UpdateTaskState(Guid taskId, Guid stateId);

		Task<int> GetChildrenAmount(Guid parentId);

		Task<int> GetChildrenAmountByState(Guid parentId, Guid stateId);

		bool IsTaskExists(Guid id);

		Task<TMS.Task> GetTaskWithChildren(Guid id);

		Task<IEnumerable<ReportTaskDTO>> GetTasksForReport(DateTime date);
	}
}