using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.DBWork;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.ObjectLogic
{
	/// <summary>
	/// Contains logic executed when task changes
	/// </summary>
	public class TaskObjectLogic
	{
		private ITaskHistoryRepository _historyRep;

		public TaskObjectLogic(ITaskHistoryRepository historyRep)
		{
			_historyRep = historyRep;
		}

		/// <summary>
		/// Executes logic for task change event
		/// </summary>
		/// <param name="changedEntities"></param>
		public async Task ExecuteAsync(IEnumerable<EntityEntry> changedEntities)
		{
			await UpdateHistory(changedEntities);
		}

		/// <summary>
		/// Updates task history if state is changed
		/// </summary>
		/// <param name="changedTasks"></param>
		private async Task UpdateHistory(IEnumerable<EntityEntry> changedTasks)
		{
			var entities = changedTasks
				  .Where(t => t.Property("StateId").IsModified).Select(e => e.Entity);

			foreach (var item in entities)
			{
				await _historyRep.UpdateHistoryAsync((TMSTask)item);
			}
		}
	}
}