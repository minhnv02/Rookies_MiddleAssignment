using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidAssignment.Application.Services.Impl
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Task<bool> CreateAsync(Cart cart);

        //Task<bool> DeleteAsync(string userId, int bookId);

        //Task<Cart> GetCartAsync(string userId, int bookId);

        //Task<IEnumerable<Cart>> GetByUserAsync(string userId);

        public async Task<bool> CreateAsync(Cart cart)
        {
            await _unitOfWork.CartRepository.AddAsync(cart);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string userId, int bookId)
        {
            var cart = await _unitOfWork.CartRepository.GetAsync(c => c.UserId == userId && c.BookId == bookId);

            _unitOfWork.CartRepository.Delete(cart);
            return await _unitOfWork.CommitAsync() > 0;
        }

        //public async Task<Cart> GetCartAsync(string userId, int bookId)
        //{
        //    return await _unitOfWork.CartRepository.GetAsync(c => c.UserId == userId && c.BookId == bookId);
        //}

        public async Task<IEnumerable<Cart>> GetByUserAsync(string userId)
        {
            return await _unitOfWork.CartRepository.GetAllAsync(c => c.UserId == userId);
        }

        public async Task<bool> DeleteByBookId(int bookId)
        {
            var carts = await _unitOfWork.CartRepository.GetAllAsync(c => c.BookId == bookId);

            foreach (var cart in carts)
            {
                _unitOfWork.CartRepository.Delete(cart);
            }

            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
