using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
	public interface ITaskService
	{
		Task<TMSTask> FindAsync(Guid id);
		Task<IEnumerable<GetTaskDTO>> GetAllAsync();
		Task<TMSTask> AddAsync(CreateTaskDTO task);
		Task<TMSTask> UpdateAsync(Guid id, UpdateTaskDTO task);
		Task<bool> RemoveAsync(Guid id);
		Task<byte[]> GetReport(DateTime date);
		bool IsTaskExists(Guid id);
	}
}