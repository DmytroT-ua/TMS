using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	/// <inheritdoc/>
	public class TaskHistoryRepository : ITaskHistoryRepository
	{
		private readonly AppDBContext _context;

		public TaskHistoryRepository(AppDBContext context)
		{
			_context = context;
		}

		/// <inheritdoc/>
		public async Task UpdateHistoryAsync(TMSTask task)
		{
			var res = await GetLastRecord(task);
			if (res != null)
			{
				res.EndDate = DateTime.UtcNow;
			}
			_context.TaskHistories.Add(GetNewRecord(task));
		}

		/// <inheritdoc/>
		public TaskHistory AddNewRecordAsync(TMSTask task)
		{
			var newRecord = GetNewRecord(task);
			_context.TaskHistories.Add(newRecord);
			return newRecord;
		}

		/// <inheritdoc/>
		public async Task CompleteAsync()
			=> await _context.SaveChangesAsync();

		/// <summary>
		/// Finds last history record.
		/// </summary>
		/// <param name="task"></param>
		/// <returns>Returns history record.</returns>
		private async Task<TaskHistory> GetLastRecord(TMSTask task)
			=> await _context.TaskHistories
				.Where(h => h.TaskId == task.Id)
				.OrderByDescending(h => h.StartDate)
				.FirstOrDefaultAsync();

		/// <summary>
		/// Creates new history record.
		/// </summary>
		/// <param name="task">Task record</param>
		/// <returns>Returns history record.</returns>
		private TaskHistory GetNewRecord(TMSTask task)
			=> new TaskHistory
			{
				StateId = task.StateId,
				TaskId = task.Id,
				StartDate = DateTime.UtcNow
			};
	}
}