using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
	/// <summary>
	/// Task history model for Task Management system
	/// </summary>
	public class TaskHistory : BaseEntity
	{
		[NotMapped]
		public override string Name { get => base.Name; set => base.Name = value; }

		public Guid TaskId { get; set; }
		public TMSTask Task {get;set;}

		public Guid? StateId { get; set; }
		public TaskState State { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}