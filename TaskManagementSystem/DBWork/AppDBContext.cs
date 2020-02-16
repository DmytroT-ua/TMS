using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem.ObjectLogic;
using TMS = TaskManagementSystem.Models;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementSystem.DBWork
{
	public class AppDBContext : DbContext
	{
		private TaskObjectLogic _taskLogic;

		private readonly IServiceProvider _container;

		public DbSet<TMS.TaskState> TaskStates { get; set; }

		public DbSet<TMS.TMSTask> Tasks { get; set; }

		public DbSet<TMS.TaskHistory> TaskHistories { get; set; }

		public AppDBContext(
			DbContextOptions<AppDBContext> options,
			IServiceProvider container)
			: base(options)
		{
			_container = container;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TMS.TaskHistory>()
				.HasOne(p => p.State)
				.WithMany(t => t.TaskHistories)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<TMS.TMSTask>()
				.HasOne(p => p.State)
				.WithMany(t => t.Tasks)
				.OnDelete(DeleteBehavior.SetNull);
		}

		public override int SaveChanges()
		{
			OnChangesSaved().Wait();
			return base.SaveChanges();
		}

		public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			await OnChangesSaved();
			return await base.SaveChangesAsync(cancellationToken);
		}

		protected async Task OnChangesSaved()
		{
			_taskLogic = _container.GetService<TaskObjectLogic>();
			var entities = GetChangedEntities();
			await _taskLogic.ExecuteAsync(GetChangedEntitiesByType(entities, typeof(TMS.TMSTask)));
		}

		protected IEnumerable<EntityEntry> GetChangedEntities()
			=> base.ChangeTracker.Entries()
				.Where(e => e.State != EntityState.Unchanged);

		protected IEnumerable<EntityEntry> GetChangedEntitiesByType(
			IEnumerable<EntityEntry> changedEntities, Type type)
				=> changedEntities.Where(x => Type.Equals(x.Entity.GetType(), type));
	}
}