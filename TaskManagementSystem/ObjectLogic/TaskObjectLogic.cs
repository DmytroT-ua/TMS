using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.DBWork;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.ObjectLogic
{
	public class TaskObjectLogic
	{
		private ITaskHistoryRepository _historyRep;

		public TaskObjectLogic(ITaskHistoryRepository historyRep)
		{
			_historyRep = historyRep;
		}

		public async Task ExecuteAsync(IEnumerable<EntityEntry> changedEntities)
		{
			await UpdateHistory(changedEntities);
		}

		private async Task UpdateHistory(IEnumerable<EntityEntry> changedTasks)
		{
			var entities = changedTasks
				  .Where(t => t.Property("StateId").IsModified).Select(e => e.Entity);

			foreach (var item in entities)
			{
				await _historyRep.UpdateHistoryAsync((TMS.Task)item);
			}
		}
	}
}