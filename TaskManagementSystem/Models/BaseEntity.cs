using System;

namespace TaskManagementSystem.Models
{
	public class BaseEntity
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public virtual string Name { get; set; }
	}
}