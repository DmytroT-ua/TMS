using System;
using System.Collections.Generic;
using TaskManagementSystem.DTO;

namespace TaskManagementSystem.Models
{
	/// <summary>
	/// Task model for Task Management system
	/// </summary>
	public class Task : BaseEntity
	{
		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public TaskState State { get; set; }
		public Guid? StateId { get; set; }

		public Task ParentTask { get; set; }
		public Guid? ParentTaskId { get; set; }

		public ICollection<Task> Children { get; set; }

		public Task() { }

		public Task(CreateTaskDTO dto)
		{
			Name = dto.Name;
			Description = dto.Description;
			StartDate = dto.StartDate;
			FinishDate = dto.FinishDate;
			StateId = dto.StateId;
			ParentTaskId = dto.ParentTaskId;
		}

		/// <summary>
		/// Updates task from UpdateTaskDTO
		/// </summary>
		/// <param name="dto">Update model</param>
		/// <param name="childrenAmount">Amount of children records</param>
		public void UpdateFromDTO(UpdateTaskDTO dto, int childrenAmount)
		{
			if (Name != dto.Name) Name = dto.Name;
			if (Description != dto.Description) Description = dto.Description;
			if (StartDate != dto.StartDate) StartDate = dto.StartDate;
			if (FinishDate != dto.FinishDate) FinishDate = dto.FinishDate;
			if (ParentTaskId != dto.ParentTaskId) ParentTaskId = dto.ParentTaskId;
			if (StateId != dto.StateId && childrenAmount == 0) StateId = dto.StateId;
		}
	}
}