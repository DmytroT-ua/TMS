using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
	public class TaskState : BaseEntity
	{
		public List<Task> Tasks { get; set; }

		public List<TaskHistory> TaskHistories { get; set; }
	}
}