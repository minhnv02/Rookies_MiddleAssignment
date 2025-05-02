using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;
using MidAssignmentProject.API.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MidAssignmentProjectTest.Controller
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoryController _categoryController;

        public CategoryControllerTest()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetCategories_WhenCategoriesFound_ReturnsOk()
        {
            // Arrange
            var categories = new List<CategoryResponse>
    {
        new CategoryResponse { Id = 1, Name = "Category 1" },
        new CategoryResponse { Id = 2, Name = "Category 2" }
    };
            _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(categories);

            // Act
            var result = await _categoryController.GetCategories(null, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get categories successfully", response.Message);
            Assert.Equal(categories, response.Data);
        }

        [Fact]
        public async Task GetCategories_WhenNameNotNull_ReturnsOk()
        {
            // Arrange
            var name = "Category 1";

            var categories = new List<CategoryResponse>
    {
        new CategoryResponse { Id = 1, Name = "Category 1" },
        new CategoryResponse { Id = 2, Name = "Category 2" },
        new CategoryResponse { Id = 2, Name = "Category 3" }
    };
            _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(categories);

            // Act
            var result = await _categoryController.GetCategories(name, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get categories successfully", response.Message);
            Assert.NotEqual(categories, response.Data);
        }

        [Fact]
        public async Task GetCategories_WhenNoCategoriesFound_ReturnsNotFound()
        {
            // Arrange
            _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(Enumerable.Empty<CategoryResponse>());

            // Act
            var result = await _categoryController.GetCategories(null, 1, 10);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No categories found", response.Message);
        }

        [Fact]
        public async Task GetCategories_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            _categoryServiceMock.Setup(x => x.GetCategories()).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _categoryController.GetCategories(null, 1, 10);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryFound_ReturnsOk()
        {
            // Arrange
            int categoryId = 1;
            var category = new CategoryResponse { Id = categoryId, Name = "Category 1" };
            _categoryServiceMock.Setup(x => x.GetCategoryById(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryController.GetCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get category successfully", response.Message);
            Assert.Equal(category, response.Data);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            int categoryId = 1;
            _categoryServiceMock.Setup(x => x.GetCategoryById(categoryId)).ReturnsAsync((CategoryResponse)null);

            // Act
            var result = await _categoryController.GetCategory(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Category not found", response.Message);
        }

        [Fact]
        public async Task GetCategory_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            int categoryId = 1;
            _categoryServiceMock.Setup(x => x.GetCategoryById(categoryId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _categoryController.GetCategory(categoryId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task CreateCategory_WhenCategoryCreated_ReturnsOk()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = "New Category" };
            _categoryServiceMock.Setup(x => x.CreateCategory(categoryRequest)).ReturnsAsync(true);

            // Act
            var result = await _categoryController.CreateCategory(categoryRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Create category successfully", response.Message);
        }

        [Fact]
        public async Task CreateCategory_WhenCategoryCreationFails_ReturnsConflict()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = "New Category" };
            _categoryServiceMock.Setup(x => x.CreateCategory(categoryRequest)).ReturnsAsync(false);

            // Act
            var result = await _categoryController.CreateCategory(categoryRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Create category failed", response.Message);
        }

        [Fact]
        public async Task CreateCategory_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = "New Category" };
            _categoryServiceMock.Setup(x => x.CreateCategory(categoryRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _categoryController.CreateCategory(categoryRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryUpdated_ReturnsOk()
        {
            // Arrange
            long categoryId = 1;
            var categoryRequest = new CategoryRequest { Name = "Updated Category" };
            _categoryServiceMock.Setup(x => x.UpdateCategory(categoryId, categoryRequest)).ReturnsAsync(true);

            // Act
            var result = await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update category successfully", response.Message);
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryUpdateFails_ReturnsConflict()
        {
            // Arrange
            long categoryId = 1;
            var categoryRequest = new CategoryRequest { Name = "Updated Category" };
            _categoryServiceMock.Setup(x => x.UpdateCategory(categoryId, categoryRequest)).ReturnsAsync(false);

            // Act
            var result = await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update category failed", response.Message);
        }

        [Fact]
        public async Task UpdateCategory_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long categoryId = 1;
            var categoryRequest = new CategoryRequest { Name = "Updated Category" };
            _categoryServiceMock.Setup(x => x.UpdateCategory(categoryId, categoryRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _categoryController.UpdateCategory(categoryId, categoryRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryDeleted_ReturnsOk()
        {
            // Arrange
            long categoryId = 1;
            long newCategoryId = 2;
            _categoryServiceMock.Setup(x => x.DeleteCategory(categoryId, newCategoryId)).ReturnsAsync(true);

            // Act
            var result = await _categoryController.DeleteCategory(categoryId, newCategoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Delete category successfully", response.Message);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryDeletionFails_ReturnsConflict()
        {
            // Arrange
            long categoryId = 1;
            long newCategoryId = 2;
            _categoryServiceMock.Setup(x => x.DeleteCategory(categoryId, newCategoryId)).ReturnsAsync(false);

            // Act
            var result = await _categoryController.DeleteCategory(categoryId, newCategoryId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Delete category failed", response.Message);
        }

        [Fact]
        public async Task DeleteCategory_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long categoryId = 1;
            long newCategoryId = 2;
            _categoryServiceMock.Setup(x => x.DeleteCategory(categoryId, newCategoryId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _categoryController.DeleteCategory(categoryId, newCategoryId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
