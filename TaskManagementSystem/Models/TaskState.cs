using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
	/// <summary>
	/// Task state model for Task Management system
	/// </summary>
	public class TaskState : BaseEntity
	{
		public List<Task> Tasks { get; set; }

		public List<TaskHistory> TaskHistories { get; set; }
	}
}