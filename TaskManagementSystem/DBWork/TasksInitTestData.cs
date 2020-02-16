using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.DBWork
{
	public class TasksInitTestData
	{
		public IEnumerable<TaskState> GetTaskStates()
			=> new TaskState[]
			{
				new TaskState { Name = "Planned" },
				new TaskState { Name = "InProgress" },
				new TaskState { Name = "Completed" }
			};

		public IEnumerable<Task> GetTasks()
		{
			var parentTaskWithoutState = new Task
			{
				Name = "Parent_level_1",
				Description = "Parent_level_1 without state",
				StartDate = DateTime.Now,
				FinishDate = DateTime.Now.AddDays(2),
			};

			var parentTaskWithState = new Task
			{
				Name = "Parent_level_1",
				Description = "Parent_level_1 with state",
				StartDate = DateTime.Now,
				FinishDate = DateTime.Now.AddDays(2),
			};

			var level2TasksWithoutState = GetTasksForParent(parentTaskWithoutState);
			var level2TasksWithState = GetTasksForParent(parentTaskWithState);

			var level3Task = new Task
			{
				Name = "Level_3",
				StartDate = DateTime.Now,
				FinishDate = DateTime.Now.AddDays(3),
				ParentTask = level2TasksWithState.FirstOrDefault()
			};

			var result = new List<Task>
			{
				parentTaskWithoutState,
				parentTaskWithState
			};
			result.AddRange(level2TasksWithoutState);
			result.AddRange(level2TasksWithState);
			result.Add(level3Task);

			return result;
		}

		public IEnumerable<Task> GetTasksForParent(Task task, int amount = 2)
		{
			for (int i = 0; i < amount; i++)
			{
				yield return new Task
				{
					Name = "Level_2",
					StartDate = DateTime.Now,
					FinishDate = DateTime.Now.AddDays(1),
					ParentTask = task,
					StateId = CONST.Task.State.Planned
				};
			}
		}
	}
}
