using System;

namespace TaskManagementSystem.Models
{
	/// <summary>
	/// Base entity model with common properties
	/// </summary>
	public class BaseEntity
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public virtual string Name { get; set; }
	}
}