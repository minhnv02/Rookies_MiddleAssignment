using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;

namespace MidAssignment.Application.Services
{
    public interface IBorrowingService
    {
        Task<bool> CreateAsync(BorrowingRequest request);

        Task<bool> UpdateStatusAsync(long id, BorrowingUpdateStatusRequest request);

        Task<BorrowingResponse> GetByIdAsync(long id);

        Task<IEnumerable<BorrowingResponse>> GetByRequestorIdAsync(string requestorId);

        //Task<IEnumerable<BorrowingResponse>> GetByApproverIdAsync(string approverId);

        Task<IEnumerable<BorrowingResponse>> GetAllAsync();

        Task<bool> DeleteAsync(long id);
    }
}
