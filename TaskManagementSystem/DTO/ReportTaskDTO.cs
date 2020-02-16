using System;

namespace TaskManagementSystem.DTO
{
	public class ReportTaskDTO
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public string State { get; set; }

		public string Parent { get; set; }
	}
}
