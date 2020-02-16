using System;
using System.ComponentModel.DataAnnotations.Schema;
using TMS = TaskManagementSystem.Models;

namespace TaskManagementSystem.Models
{
	public class TaskHistory : BaseEntity
	{
		[NotMapped]
		public override string Name { get => base.Name; set => base.Name = value; }

		public Guid TaskId { get; set; }
		public TMS.Task Task {get;set;}

		public Guid? StateId { get; set; }
		public TMS.TaskState State { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}