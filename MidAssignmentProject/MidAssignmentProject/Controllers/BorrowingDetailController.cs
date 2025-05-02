using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/borrowing-details")]
    [ApiController]
    public class BorrowingDetailController : BaseApiController
    {
        private readonly IBorrowingDetailService _borrowingDetailService;

        public BorrowingDetailController(IBorrowingDetailService borrowingDetailService)
        {
            _borrowingDetailService = borrowingDetailService;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateAsync(List<BorrowingDetailRequest> request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.CreateAsync(request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Create borrowing detail failed";
                    return Conflict(response);
                }
                response.Message = "Create borrowing detail successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("update-status/{borrowingId}/{bookId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatusAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.UpdateStatusAsync(borrowingId, bookId, request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update status borrowing detail failed";
                    return Conflict(response);
                }
                response.Message = "Update status borrowing detail successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("update-status-extend/{borrowingId}/{bookId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateStatusExtendAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.UpdateStatusExtendAsync(borrowingId, bookId, request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update status extend borrowing detail failed";
                    return Conflict(response);
                }
                response.Message = "Update status extend borrowing detail successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        //[HttpPut("update-returned-at/{borrowingId}/{bookId}")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> UpdateReturnedAtAsync(long borrowingId, long bookId, BorrowingDetailUpdateReturnedAtRequest request)
        //{
        //    var response = new GeneralResponse();
        //    try
        //    {
        //        var result = await _borrowingDetailService.UpdateReturnedAtAsync(borrowingId, bookId, request);
        //        if (!result)
        //        {
        //            response.Success = false;
        //            response.Message = "Update returned at borrowing detail failed";
        //            return Conflict(response);
        //        }
        //        response.Message = "Update returned at borrowing detail successfully";
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return Conflict(response);
        //    }
        //}

        [HttpDelete("{borrowingId}/{bookId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteAsync(long borrowingId, long bookId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.DeleteAsync(borrowingId, bookId);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Delete borrowing detail failed";
                    return Conflict(response);
                }
                response.Message = "Delete borrowing detail successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("borrowing/{borrowingId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetBorrowingDetailsByBorrowingIdAsync(long borrowingId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.GetBorrowingDetailsByBorrowingIdAsync(borrowingId);
                if (result == null || !result.Any())
                {
                    response.Success = false;
                    response.Message = "No borrowing detail found";
                    return NotFound(response);
                }
                response.Message = "Get borrowing details by borrowing id successfully";
                response.Data = result.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("handle-extension/{borrowingId}/{bookId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> HandleExtensionAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.HandleExtension(borrowingId, bookId, request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Handle extension borrowing detail failed";
                    return Conflict(response);
                }
                response.Message = "Handle extension borrowing detail successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("request-extend")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetBorrowingDetailsRequestExtend()
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingDetailService.GetBorrowingDetailsRequestExtend();
                if (result == null || !result.Any())
                {
                    response.Success = false;
                    response.Message = "No borrowing detail found";
                    return NotFound(response);
                }
                response.Message = "Get borrowing details request extend successfully";
                response.Data = result.ToList();
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
