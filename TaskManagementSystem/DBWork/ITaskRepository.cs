using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.DTO;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	/// <summary>
	/// Contains methods to work with Task.
	/// </summary>
	public interface ITaskRepository
	{
		/// <summary>
		/// Finds task by id
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns task</returns>
		Task<TMS.TMSTask> FindAsync(Guid id);

		/// <summary>
		/// Gets all tasks.
		/// </summary>
		/// <returns>Returns all tasks.</returns>
		Task<IEnumerable<GetTaskDTO>> GetAllAsync();

		/// <summary>
		/// Creates new task.
		/// </summary>
		/// <param name="task"></param>
		/// <returns>Returns created task.</returns>
		Task<TMS.TMSTask> AddAsync(CreateTaskDTO task);

		/// <summary>
		/// Updates task by id.
		/// </summary>
		/// <param name="task">Task dto</param>
		/// <param name="taskEntity">Current task model</param>
		Task UpdateAsync(UpdateTaskDTO task, TMS.TMSTask taskEntity);


		/// <summary>
		/// Removes task.
		/// </summary>
		/// <param name="task">Task entity</param>
		/// <returns>Returns removed task.</returns>
		Task<TMS.TMSTask> RemoveAsync(TMS.TMSTask task);

		/// <summary>
		/// Finds parent task id
		/// </summary>
		/// <param name="taskId">Task id</param>
		/// <returns>Returns task id</returns>
		Task<Guid> GetParentTaskId(Guid taskId);

		/// <summary>
		/// Updates task state
		/// </summary>
		/// <param name="taskId">Task id</param>
		/// <param name="stateId">Task state id</param>
		/// <returns>Returns task id</returns>
		Task<Guid> UpdateTaskState(Guid taskId, Guid stateId);

		/// <summary>
		/// Calculate amount of children tasks
		/// </summary>
		/// <param name="parentId">Parent task id</param>
		/// <returns>Returns amount of children tasks</returns>
		Task<int> GetChildrenAmount(Guid parentId);

		/// <summary>
		/// Calculate amount of children tasks with specific type
		/// </summary>
		/// <param name="parentId">Parent task id</param>
		/// <param name="stateId">Task state id</param>
		/// <returns>Returns amount of children tasks</returns>
		Task<int> GetChildrenAmountByState(Guid parentId, Guid stateId);

		/// <summary>
		/// Check if task exists.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Returns is task exists</returns>
		bool IsTaskExists(Guid id);

		/// <summary>
		/// Loads task with children
		/// </summary>
		/// <param name="id">Task id</param>
		/// <returns>Returns task with children</returns>
		Task<TMS.TMSTask> GetTaskWithChildren(Guid id);

		/// <summary>
		/// Loads tasks in progress for report
		/// </summary>
		/// <param name="date"></param>
		/// <returns>Returns report dto for tasks</returns>
		Task<IEnumerable<ReportTaskDTO>> GetTasksForReport(DateTime date);
	}
}