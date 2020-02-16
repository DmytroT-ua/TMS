using System;

namespace TaskManagementSystem
{
	public static class CONST
	{
		public static class Task
		{
			public static class State
			{
				public static readonly Guid Planned = new Guid("05eb2d0a-b55f-462d-a83b-8da91b78acef");
				public static readonly Guid InProgress = new Guid("4ed9da6e-121f-454e-bc2f-c7e0a995857c");
				public static readonly Guid Completed = new Guid("383056ca-04b6-420d-9564-e751e9e3faa9");
			}
		}
	}
}
