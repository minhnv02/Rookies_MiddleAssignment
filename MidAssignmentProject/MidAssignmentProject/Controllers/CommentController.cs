using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetComments()
        //{
        //    var response = new GeneralResponse();
        //    try
        //    {
        //        var comments = await _commentService.GetComments();
        //        if (comments == null || !comments.Any())
        //        {
        //            response.Success = false;
        //            response.Message = "No comments found";
        //            return NotFound(response);
        //        }
        //        response.Message = "Get comments successfully";
        //        response.Data = comments.ToList();
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return Conflict(response);
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetComment(long id)
        //{
        //    var response = new GeneralResponse();
        //    try
        //    {
        //        var comment = await _commentService.GetCommentById(id);
        //        if (comment == null)
        //        {
        //            response.Success = false;
        //            response.Message = "Comment not found";
        //            return NotFound(response);
        //        }
        //        response.Message = "Get comment successfully";
        //        response.Data = comment;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return Conflict(response);
        //    }
        //}

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateComment(CommentRequest comment)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _commentService.CreateComment(comment);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Create fail";
                    return Conflict(response);
                }
                response.Message = "Create comment successfully";
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
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateComment(long id, CommentRequest comment)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _commentService.UpdateComment(id, comment);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update fail";
                    return Conflict(response);
                }
                response.Message = "Update comment successfully";
                response.Data = id;
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
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> DeleteComment(long id)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _commentService.DeleteComment(id);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Delete fail";
                    return Conflict(response);
                }
                response.Message = "Delete comment successfully";
                response.Data = id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(string userId)
        {
            var response = new GeneralResponse();
            try
            {
                var comments = await _commentService.GetCommentsByUserId(userId);
                if (comments == null || !comments.Any())
                {
                    response.Success = false;
                    response.Message = "No comments found";
                    return NotFound(response);
                }
                response.Message = "Get comments successfully";
                response.Data = comments.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetCommentsByBookId(long bookId)
        {
            var response = new GeneralResponse();
            try
            {
                var comments = await _commentService.GetCommentsByBookId(bookId);
                if (comments == null || !comments.Any())
                {
                    response.Success = false;
                    response.Message = "No comments found";
                    return NotFound(response);
                }
                response.Message = "Get comments successfully";
                response.Data = comments.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        //get newest comments of user
        [HttpGet("user/{userId}/newest")]
        public async Task<IActionResult> GetNewestCommentsByUserId(string userId)
        {
            var response = new GeneralResponse();
            try
            {
                var comments = await _commentService.GetNewestCommentsByUserId(userId);
                if (comments == null)
                {
                    response.Success = false;
                    response.Message = "No comments found";
                    return NotFound(response);
                }

                response.Message = "Get comments successfully";
                response.Data = comments;
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
