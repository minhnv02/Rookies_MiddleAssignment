using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : BaseApiController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetRatingsByBookId(long bookId)
        {
            var response = new GeneralResponse();
            try
            {
                var ratings = await _ratingService.GetRatingsByBookId(bookId);
                if (ratings == null || !ratings.Any())
                {
                    response.Success = false;
                    response.Message = "No ratings found";
                    return NotFound(response);
                }
                response.Message = "Get ratings successfully";
                response.Data = ratings.ToList();
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
