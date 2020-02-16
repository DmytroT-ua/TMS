using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	public class TaskHistoryRepository : ITaskHistoryRepository
	{
		private readonly AppDBContext _context;

		public TaskHistoryRepository(AppDBContext context)
		{
			_context = context;
		}

		public async Task UpdateHistoryAsync(TMS.Task task)
		{
			var res = await GetLastRecord(task);
			if (res != null)
			{
				res.EndDate = DateTime.UtcNow;
			}
			_context.TaskHistories.Add(GetNewRecord(task));
		}

		public TMS.TaskHistory AddNewRecordAsync(TMS.Task task)
		{
			var newRecord = GetNewRecord(task);
			_context.TaskHistories.Add(newRecord);
			return newRecord;
		}

		public async Task CompleteAsync()
			=> await _context.SaveChangesAsync();

		private async Task<TMS.TaskHistory> GetLastRecord(TMS.Task task)
			=> await _context.TaskHistories
				.Where(h => h.TaskId == task.Id)
				.OrderByDescending(h => h.StartDate)
				.FirstOrDefaultAsync();

		private TMS.TaskHistory GetNewRecord(TMS.Task task)
			=> new TMS.TaskHistory
			{
				StateId = task.StateId,
				TaskId = task.Id,
				StartDate = DateTime.UtcNow
			};
	}
}