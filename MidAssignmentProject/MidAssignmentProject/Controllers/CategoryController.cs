using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new GeneralResponse();
            try
            {
                var categories = await _categoryService.GetCategories();
                if (categories == null || !categories.Any())
                {
                    response.Success = false;
                    response.Message = "No categories found";
                    return NotFound(response);
                }
                if (!string.IsNullOrEmpty(name))
                    categories = categories.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                categories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                response.Message = "Get categories successfully";
                response.Data = categories.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var response = new GeneralResponse();
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category not found";
                    return NotFound(response);
                }
                response.Message = "Get category successfully";
                response.Data = category;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory(CategoryRequest categoryRequest)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _categoryService.CreateCategory(categoryRequest);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Create category failed";
                    return Conflict(response);
                }
                response.Message = "Create category successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("{cateId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(long cateId, CategoryRequest categoryRequest)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _categoryService.UpdateCategory(cateId, categoryRequest);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update category failed";
                    return Conflict(response);
                }
                response.Message = "Update category successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpDelete("{cateId}/{newCateId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(long cateId, long newCateId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _categoryService.DeleteCategory(cateId, newCateId);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Delete category failed";
                    return Conflict(response);
                }
                response.Message = "Delete category successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }
    }
}
