using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TMS_Tests
{
	class TaskServiceFake : ITaskService
	{
		private readonly List<TMSTask> _taskRep;

		public TaskServiceFake()
		{
			var task1 = new TMSTask { Id = new Guid("F4272DB4-A8F8-4C99-B985-68B1D61BA0C2"), Name = "Leve_1", 
				FinishDate = DateTime.UtcNow.AddDays(2), StartDate = DateTime.UtcNow, StateId = CONST.Task.State.InProgress };
			var task2 = new TMSTask { Id = new Guid("7C4E308E-1ED4-43FA-B380-3BCE91BACD95"), Name = "Leve_2", 
				FinishDate = DateTime.UtcNow.AddDays(2), StartDate = DateTime.UtcNow, StateId = CONST.Task.State.Planned, ParentTaskId = task1.Id};
			var task22 = new TMSTask { Id = new Guid("B19F14A8-4944-4124-9CA6-43B06946DB73"), Name = "Leve_2", 
				FinishDate = DateTime.UtcNow.AddDays(2), StartDate = DateTime.UtcNow, StateId = CONST.Task.State.InProgress, ParentTaskId = task1.Id};
			var task3 = new TMSTask { Id = new Guid("58F3DED0-EE1C-4322-B290-6E11DC7729A0"), Name = "Leve_3", 
				FinishDate = DateTime.UtcNow.AddDays(2), StartDate = DateTime.UtcNow, StateId = CONST.Task.State.InProgress, ParentTaskId = task22.Id };
			_taskRep = new List<TMSTask> 
			{
				task1, task2, task22, task3
			};
		}

		public async Task<TMSTask> AddAsync(CreateTaskDTO task)
			=> await Task.Run(() =>
			{
				var newTask = new TMSTask(task);
				_taskRep.Add(newTask);
				return newTask;
			});

		public async Task<TMSTask> FindAsync(Guid id)
			=> await Task.Run(() => _taskRep.Where(t => t.Id == id).SingleOrDefault());

		public async Task<IEnumerable<GetTaskDTO>> GetAllAsync() 
			=> await Task.Run(() => _taskRep.Select(t => new GetTaskDTO(t)).ToList());

		public bool IsTaskExists(Guid id)
			=> _taskRep.Any(t => t.Id == id);

		public async Task<bool> RemoveAsync(Guid id)
			=> await Task.Run(async () => 
			{
				bool result = default;
				var task = await FindAsync(id);
				if (task != null)
				{
					_taskRep.Remove(task);
					result = true;
				}
				return result;
			});

		public async Task<TMSTask> UpdateAsync(Guid id, UpdateTaskDTO taskDTO)
			=> await Task.Run(async () =>
			{
				var task = await FindAsync(id);
				if (task != null)
				{
					task.Name = taskDTO.Name;
					task.Description = taskDTO.Description;
					task.ParentTaskId = taskDTO.ParentTaskId;
					task.StartDate = taskDTO.StartDate;
					task.FinishDate = taskDTO.FinishDate;
					task.StateId = taskDTO.StateId;
				}
				return task;
			});

		public Task<byte[]> GetReport(DateTime date)
			=> throw new NotImplementedException();
	}
}