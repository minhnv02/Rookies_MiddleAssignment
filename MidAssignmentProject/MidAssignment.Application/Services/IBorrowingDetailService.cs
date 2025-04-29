using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;

namespace MidAssignment.Application.Services
{
    public interface IBorrowingDetailService
    {
        Task<bool> CreateAsync(List<BorrowingDetailRequest> request);

        Task<bool> UpdateStatusAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusRequest request);

        Task<bool> UpdateStatusExtendAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request);

        Task<bool> HandleExtension(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request);

        //Task<bool> UpdateReturnedAtAsync(long borrowingId, long bookId, BorrowingDetailUpdateReturnedAtRequest request);

        Task<bool> DeleteAsync(long borrowingId, long bookId);

        Task<IEnumerable<BorrowingDetailResponse>> GetBorrowingDetailsByBorrowingIdAsync(long borrowingId);

        Task<IEnumerable<BorrowingDetailResponse>> GetBorrowingDetailsRequestExtend();
    }
}
