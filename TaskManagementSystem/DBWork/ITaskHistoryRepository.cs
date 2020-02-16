using System.Threading.Tasks;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	/// <summary>
	/// Contains methods to work with TaskHistory.
	/// </summary>
	public interface ITaskHistoryRepository
	{
		/// <summary>
		/// Updates last history record and creates next record.
		/// </summary>
		/// <param name="task"></param>
		Task UpdateHistoryAsync(TMS.TMSTask task);

		/// <summary>
		/// Creates new history record.
		/// </summary>
		/// <param name="task"></param>
		/// <returns>Returns created record.</returns>
		TMS.TaskHistory AddNewRecordAsync(TMS.TMSTask task);

		/// <summary>
		/// Executes SaveChanges.
		/// </summary>
		Task CompleteAsync();
	}
}