using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;

namespace MidAssignment.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetCategories();

        Task<CategoryResponse> GetCategoryById(long id);

        Task<bool> CreateCategory(CategoryRequest categoryRequest);

        Task<bool> UpdateCategory(long cateId, CategoryRequest categoryRequest);

        Task<bool> DeleteCategory(long cateId, long newCateId);
    }
}
