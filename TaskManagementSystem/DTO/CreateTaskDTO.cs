using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.DTO
{
	public class CreateTaskDTO
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public Guid? StateId { get; set; }

		public Guid? ParentTaskId { get; set; }
	}
}