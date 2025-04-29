using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateAsync(Cart cart)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _cartService.CreateAsync(cart);

                if (!result)
                {
                    response.Success = false;
                    response.Message = "Book already exists in cart";
                    return BadRequest(response);
                }

                response.Success = true;
                response.Message = "Book added to cart";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpDelete("{userId}/{bookId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteAsync(string userId, int bookId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _cartService.DeleteAsync(userId, bookId);

                if (!result)
                {
                    response.Success = false;
                    response.Message = "Book not found in cart";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Book removed from cart";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        //[HttpGet("{userId}/{bookId}")]
        //[Authorize(Roles = "admin,user")]
        //public async Task<IActionResult> GetCartAsync(string userId, int bookId)
        //{
        //    var response = new GeneralResponse();
        //    try
        //    {
        //        var cart = await _cartService.GetCartAsync(userId, bookId);

        //        if (cart == null)
        //        {
        //            response.Success = false;
        //            response.Message = "Book not found in cart";
        //            return NotFound(response);
        //        }

        //        response.Success = true;
        //        response.Message = "Get cart successfully";
        //        response.Data = cart;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return Conflict(response);
        //    }
        //}

        [HttpGet("{userId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetByUserAsync(string userId)
        {
            var response = new GeneralResponse();
            try
            {
                var carts = await _cartService.GetByUserAsync(userId);

                if (carts == null || !carts.Any())
                {
                    response.Success = false;
                    response.Message = "Cart is empty";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Get cart successfully";
                response.Data = carts;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpDelete("{bookId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteByBookId(int bookId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _cartService.DeleteByBookId(bookId);

                if (!result)
                {
                    response.Success = false;
                    response.Message = "Book not found in cart";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Book removed from cart";
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
