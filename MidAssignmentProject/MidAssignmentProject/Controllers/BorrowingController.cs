using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/borrowings")]
    [ApiController]
    public class BorrowingController : BaseApiController
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateBorrowing(BorrowingRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingService.CreateAsync(request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Create borrowing failed";
                    return Conflict(response);
                }
                response.Message = "Create borrowing successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBorrowingStatus(long id, BorrowingUpdateStatusRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingService.UpdateStatusAsync(id, request);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update borrowing status failed";
                    return Conflict(response);
                }
                response.Message = "Update borrowing status successfully";
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
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetBorrowingById(long id)
        {
            var response = new GeneralResponse();
            try
            {
                var borrowing = await _borrowingService.GetByIdAsync(id);
                if (borrowing == null)
                {
                    response.Success = false;
                    response.Message = "No borrowing found";
                    return NotFound(response);
                }
                response.Message = "Get borrowing successfully";
                response.Data = borrowing;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("requestor/{requestorId}")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetBorrowingsByRequestorId(string requestorId)
        {
            var response = new GeneralResponse();
            try
            {
                var borrowings = await _borrowingService.GetByRequestorIdAsync(requestorId);
                if (borrowings == null || !borrowings.Any())
                {
                    response.Success = false;
                    response.Message = "No borrowing found";
                    return NotFound(response);
                }
                response.Message = "Get borrowings successfully";
                response.Data = borrowings.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        //[HttpGet("approver/{approverId}")]
        //[Authorize(Roles = "user,admin")]
        //public async Task<IActionResult> GetBorrowingsByApproverId(string approverId)
        //{
        //    var response = new GeneralResponse();
        //    try
        //    {
        //        var borrowings = await _borrowingService.GetByApproverIdAsync(approverId);
        //        if (borrowings == null || !borrowings.Any())
        //        {
        //            response.Success = false;
        //            response.Message = "No borrowing found";
        //            return NotFound(response);
        //        }
        //        response.Message = "Get borrowings successfully";
        //        response.Data = borrowings.ToList();
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return Conflict(response);
        //    }
        //}

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetBorrowings()
        {
            var response = new GeneralResponse();
            try
            {
                var borrowings = await _borrowingService.GetAllAsync();
                if (borrowings == null || !borrowings.Any())
                {
                    response.Success = false;
                    response.Message = "No borrowing found";
                    return NotFound(response);
                }
                response.Message = "Get borrowings successfully";
                response.Data = borrowings.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteBorrowing(long id)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _borrowingService.DeleteAsync(id);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Delete borrowing failed";
                    return Conflict(response);
                }
                response.Message = "Delete borrowing successfully";
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
