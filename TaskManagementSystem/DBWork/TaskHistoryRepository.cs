using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TMS = TaskManagementSystem.Models;

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
		public async Task UpdateHistoryAsync(TMS.Task task)
		{
			var res = await GetLastRecord(task);
			if (res != null)
			{
				res.EndDate = DateTime.UtcNow;
			}
			_context.TaskHistories.Add(GetNewRecord(task));
		}

		/// <inheritdoc/>
		public TMS.TaskHistory AddNewRecordAsync(TMS.Task task)
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
		private async Task<TMS.TaskHistory> GetLastRecord(TMS.Task task)
			=> await _context.TaskHistories
				.Where(h => h.TaskId == task.Id)
				.OrderByDescending(h => h.StartDate)
				.FirstOrDefaultAsync();

		/// <summary>
		/// Creates new history record.
		/// </summary>
		/// <param name="task">Task record</param>
		/// <returns>Returns history record.</returns>
		private TMS.TaskHistory GetNewRecord(TMS.Task task)
			=> new TMS.TaskHistory
			{
				StateId = task.StateId,
				TaskId = task.Id,
				StartDate = DateTime.UtcNow
			};
	}
}