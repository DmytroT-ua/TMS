﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	/// <inheritdoc/>
	public class TaskRepository : ITaskRepository
	{
		private readonly AppDBContext _context;

		public TaskRepository(AppDBContext context)
		{
			_context = context;
		}

		/// <inheritdoc/>
		public async Task<TMSTask> FindAsync(Guid id)
			=> await _context.Tasks.FindAsync(id);

		/// <inheritdoc/>
		public async Task<IEnumerable<GetTaskDTO>> GetAllAsync()
			=> await _context.Tasks
				.Select(t => new GetTaskDTO(t))
				.ToListAsync();

		/// <inheritdoc/>
		public async Task<TMSTask> AddAsync(CreateTaskDTO dto)
		{
			var taskToAdd = new TMSTask(dto);

			_context.Tasks.Add(taskToAdd);
			await _context.SaveChangesAsync();

			return taskToAdd;
		}

		/// <inheritdoc/>
		public async Task UpdateAsync(UpdateTaskDTO dto, TMSTask taskEntity)
		{
			taskEntity.UpdateFromDTO(dto, await GetChildrenAmount(taskEntity.Id));
			await _context.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task<TMSTask> RemoveAsync(TMSTask task)
		{
			_context.Tasks.Remove(task);
			await _context.SaveChangesAsync();
			return task;
		}

		/// <inheritdoc/>
		public async Task<Guid> GetParentTaskId(Guid taskId)
			=> await _context.Tasks
				.Where(t => t.Id == taskId)
				.Select(t => t.ParentTaskId)
				.FirstOrDefaultAsync() ?? Guid.Empty;

		/// <inheritdoc/>
		public async Task<Guid> UpdateTaskState(Guid taskId, Guid stateId)
		{
			var task = await FindAsync(taskId);
			task.StateId = stateId;
			await _context.SaveChangesAsync();
			return task.Id;
		}

		/// <inheritdoc/>
		public async Task<int> GetChildrenAmount(Guid parentId)
			=> await _context.Tasks
				.Where(t => t.ParentTaskId == parentId)
				.CountAsync();

		/// <inheritdoc/>
		public async Task<int> GetChildrenAmountByState(Guid parentId, Guid stateId)
			=> await _context.Tasks
				.Where(t => t.ParentTaskId == parentId && t.StateId == stateId)
				.CountAsync();

		/// <inheritdoc/>
		public bool IsTaskExists(Guid id)
			=> _context.Tasks.Any(e => e.Id == id);

		/// <inheritdoc/>
		public async Task<TMSTask> GetTaskWithChildren(Guid id)
			=> await _context.Tasks
				.Include(t => t.Children)
				.SingleOrDefaultAsync(t => t.Id == id);

		/// <inheritdoc/>
		public async Task<IEnumerable<ReportTaskDTO>> GetTasksForReport(DateTime date)
		{
			var data = from task in _context.Tasks
					   join history in _context.TaskHistories on task.Id equals history.TaskId
					   join state in _context.TaskStates on task.StateId equals state.Id
					   join parent in _context.Tasks on task.ParentTaskId equals parent.Id
					   where history.StateId == CONST.Task.State.InProgress &&
						  (history.StartDate <= date && (history.EndDate >= date || history.EndDate == null))
					   select new ReportTaskDTO
					   {
						   Name = task.Name,
						   Description = task.Description,
						   FinishDate = task.FinishDate,
						   StartDate = task.StartDate,
						   State = state.Name,
						   Parent = parent.Name
					   };
			return await data.ToListAsync();
		}
	}
}