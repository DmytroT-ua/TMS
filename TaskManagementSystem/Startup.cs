using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagementSystem.DBWork;
using TaskManagementSystem.ObjectLogic;
using TaskManagementSystem.Services;

namespace TaskManagementSystem
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddTransient<ITaskHistoryRepository, TaskHistoryRepository>();
			services.AddTransient<TaskObjectLogic>();
			services.AddTransient<ITaskRepository, TaskRepository>();
			services.AddTransient<TaskService>();


			services.AddDbContext<AppDBContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("TMS_Connection")));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}