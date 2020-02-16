using System.Threading.Tasks;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	public interface ITaskHistoryRepository
	{
		Task UpdateHistoryAsync(TMS.Task task);

		TMS.TaskHistory AddNewRecordAsync(TMS.Task task);

		Task CompleteAsync();
	}
}