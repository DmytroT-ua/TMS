using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;
using Xunit;

namespace TMS_Tests
{
	public class TaskControllerTest
	{
		TaskController _controller;
		ITaskService _service;

		public TaskControllerTest()
		{
			_service = new TaskServiceFake();
			_controller = new TaskController(_service);
		}

		[Fact]
		public async void GetAll_WhenCalled_ReturnsAllItems()
		{
			var resultItems = await _controller.GetTasks();
			Assert.Equal(4, resultItems.Count());
		}

		[Fact]
		public async void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
		{
			var notFoundResult = await _controller.GetTask(Guid.NewGuid());
			Assert.IsType<NotFoundResult>(notFoundResult.Result);
		}

		[Fact]
		public async void GetById_ExistingGuidPassed_ReturnsRightItem()
		{
			var testGuid = new Guid("F4272DB4-A8F8-4C99-B985-68B1D61BA0C2");
			var result = await _controller.GetTask(testGuid);
			Assert.Equal(testGuid, (result.Value as TMSTask).Id);
		}

		[Fact]
		public async void Add_InvalidObjectPassed_ReturnsBadRequest()
		{
			var nameMissingTask = new CreateTaskDTO()
			{
				Description = "Desc",
				StateId = null
			};
			_controller.ModelState.AddModelError("Name", "Required");
			var badRequest = await _controller.PostTask(nameMissingTask);
			Assert.IsType<BadRequestObjectResult>(badRequest.Result);
		}

		[Fact]
		public async void Add_ValidObjectPassed_ReturnsCreatedResponse()
		{
			CreateTaskDTO testItem = new CreateTaskDTO()
			{
				Name = "Test name",
				Description = "Description"
			};
			var createdResponse = await _controller.PostTask(testItem);
			Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
		}

		[Fact]
		public async void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
		{
			string taskName = "Test name";
			CreateTaskDTO testItem = new CreateTaskDTO()
			{
				Name = taskName,
				Description = "Description"
			};
			var response = await _controller.PostTask(testItem);
			var createdResponse = response.Result as CreatedAtActionResult;
			var item = createdResponse.Value as TMSTask;

			Assert.IsType<TMSTask>(item);
			Assert.Equal(taskName, item.Name);
		}

		[Fact]
		public async void Remove_NotExistingGuidPassed_ReturnsConflict()
		{
			var notExistingGuid = Guid.NewGuid();
			var conflict = await _controller.DeleteTask(notExistingGuid);
			Assert.IsType<ConflictResult>(conflict);
		}

		[Fact]
		public async void Remove_ExistingGuidPassed_ReturnsOkResult()
		{
			var existingGuid = new Guid("F4272DB4-A8F8-4C99-B985-68B1D61BA0C2");
			var okResult = await _controller.DeleteTask(existingGuid);
			Assert.IsType<OkResult>(okResult);
		}

		[Fact]
		public async void Remove_ExistingGuidPassed_RemovesOneItem()
		{
			var existingGuid = new Guid("F4272DB4-A8F8-4C99-B985-68B1D61BA0C2");
			var okResponse = await _controller.DeleteTask(existingGuid);
			Assert.Equal(3, (await _service.GetAllAsync()).Count());
		}
	}
}