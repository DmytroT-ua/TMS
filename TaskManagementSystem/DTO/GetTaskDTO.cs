using System;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.DTO
{
	public class GetTaskDTO
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public Guid? StateId { get; set; }

		public Guid? ParentTaskId { get; set; }

		public GetTaskDTO(TMS.Task task)
		{
			Id = task.Id;
			Description = task.Description;
			FinishDate = task.FinishDate;
			Name = task.Name;
			ParentTaskId = task.ParentTaskId;
			StartDate = task.StartDate;
			StateId = task.StateId;
		}
	}
}