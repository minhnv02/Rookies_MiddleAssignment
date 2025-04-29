using AutoMapper;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidAssignment.Application.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateCategory(CategoryRequest categoryRequest)
        {
            var category = new Category
            {
                Name = categoryRequest.Name
            };
            await _unitOfWork.CategoryRepository.AddAsync(category);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> UpdateCategory(long cateId, CategoryRequest categoryRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => !c.IsDeleted && c.Id == cateId);
            if (category == null)
                return false;
            category.Name = categoryRequest.Name;
            _unitOfWork.CategoryRepository.Update(category);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(c => !c.IsDeleted, c => c.Books);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetCategoryById(long id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id, c => c.Books);
            if (category == null)
                return null;
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<bool> DeleteCategory(long cateId, long newCateId)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(c => !c.IsDeleted && c.Id == cateId);
            if (category == null)
                return false;
            var books = await _unitOfWork.BookRepository.GetAllAsync(b => b.CategoryId == cateId);
            foreach (var book in books)
            {
                book.CategoryId = newCateId;
                _unitOfWork.BookRepository.Update(book);
            }
            _unitOfWork.CategoryRepository.SoftDelete(category);
            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
