using AutoMapper;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using MidAssignment.Infrastructure.Services;

namespace MidAssignment.Application.Services.Impl
{
    public class BorrowingDetailService : IBorrowingDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BorrowingDetailService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<bool> CreateAsync(List<BorrowingDetailRequest> request)
        {
            var borrowing = await _unitOfWork.BorrowingRepository.GetAsync(b => b.Id == request[0].BorrowingId);
            if (borrowing == null)
            {
                return false;
            }

            var borrowingDetails = await _unitOfWork.BorrowingDetailRepository.GetAllAsync(b => !b.IsDeleted && b.BorrowingId == borrowing.Id);
            if (borrowingDetails.Count() + request.Count > 5)
            {
                throw new IOException("Don't exceed 5 borrowed books per request borrow");
            }

            foreach (var item in request)
            {
                var borrowingExist = await _unitOfWork.BorrowingDetailRepository.GetAsync(b => b.BorrowingId == item.BorrowingId && b.BookId == item.BookId);
                if (borrowingExist != null)
                {
                    borrowingExist.IsDeleted = false;
                    borrowingExist.CreatedAt = DateTime.Now;
                    borrowingExist.ReturnedAt = item.ReturnedAt;
                    _unitOfWork.BorrowingDetailRepository.Update(borrowingExist);
                }
                else
                {
                    var borrowingDetail = new BorrowingDetail
                    {
                        BorrowingId = item.BorrowingId,
                        BookId = item.BookId,
                        CreatedAt = DateTime.Now,
                        ReturnedAt = item.ReturnedAt
                    };
                    await _unitOfWork.BorrowingDetailRepository.AddAsync(borrowingDetail);
                }
            }

            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> UpdateStatusAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusRequest request)
        {
            var borrowingDetail = await _unitOfWork.BorrowingDetailRepository.GetAsync(b => !b.IsDeleted && b.BorrowingId == borrowingId && b.BookId == bookId);
            if (borrowingDetail == null)
            {
                return false;
            }
            borrowingDetail.Status = request.Status;
            _unitOfWork.BorrowingDetailRepository.Update(borrowingDetail);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> UpdateStatusExtendAsync(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request)
        {
            var borrowingDetail = await _unitOfWork
                .BorrowingDetailRepository
                .GetAsync(b => !b.IsDeleted && b.BorrowingId == borrowingId && b.BookId == bookId
                , b => b.Borrowing, b => b.Borrowing.Requestor, b => b.Borrowing.Approver);
            if (borrowingDetail == null)
            {
                return false;
            }
            borrowingDetail.StatusExtend = request.StatusExtend;
            _unitOfWork.BorrowingDetailRepository.Update(borrowingDetail);
            if (await _unitOfWork.CommitAsync() <= 0)
            {
                return false;
            }

            string subject = EmailConstants.SUBJECT_EXTEND_BORROWING;
            string content = EmailConstants.BodyExtendBorrowingEmail(borrowingId, bookId);
            string emailTo = borrowingDetail.Borrowing?.Approver?.Email ?? string.Empty;

            await _emailService.SendEmailAsync(emailTo, subject, content);
            return true;
        }

        public async Task<bool> HandleExtension(long borrowingId, long bookId, BorrowingDetailUpdateStatusExtendRequest request)
        {
            var borrowingDetail = await _unitOfWork
                .BorrowingDetailRepository
                .GetAsync(b => !b.IsDeleted && b.BorrowingId == borrowingId && b.BookId == bookId
                , b => b.Borrowing, b => b.Borrowing.Requestor, b => b.Borrowing.Approver);
            if (borrowingDetail == null)
            {
                return false;
            }
            borrowingDetail.StatusExtend = request.StatusExtend;
            borrowingDetail.ReturnedAt = request.ReturnedAt.HasValue ? request.ReturnedAt.Value : borrowingDetail.ReturnedAt;
            _unitOfWork.BorrowingDetailRepository.Update(borrowingDetail);
            if (await _unitOfWork.CommitAsync() <= 0)
            {
                return false;
            }
            string subject = string.Empty;
            string content = string.Empty;
            string emailTo = borrowingDetail.Borrowing?.Requestor?.Email ?? string.Empty;

            if (request.StatusExtend == StatusBorrowingExtend.APPROVED)
            {
                subject = EmailConstants.SUBJECT_APPROVE_EXTENSION;
                content = EmailConstants.BodyApproveExtensionEmail(borrowingId, bookId);
            }
            else if (request.StatusExtend == StatusBorrowingExtend.REJECTED)
            {
                subject = EmailConstants.SUBJECT_REJECT_EXTENSION;
                content = EmailConstants.BodyRejectExtensionEmail(borrowingId, bookId);
            }

            await _emailService.SendEmailAsync(emailTo, subject, content);
            return true;
        }

        //public async Task<bool> UpdateReturnedAtAsync(long borrowingId, long bookId, BorrowingDetailUpdateReturnedAtRequest request)
        //{
        //    var borrowingDetail = await _unitOfWork
        //        .BorrowingDetailRepository
        //        .GetAsync(b => !b.IsDeleted && b.BorrowingId == borrowingId && b.BookId == bookId
        //        , b => b.Borrowing, b => b.Borrowing.Requestor);
        //    if (borrowingDetail == null)
        //    {
        //        return false;
        //    }
        //    borrowingDetail.ReturnedAt = request.ReturnedAt;
        //    _unitOfWork.BorrowingDetailRepository.Update(borrowingDetail);
        //    if (await _unitOfWork.CommitAsync() <= 0)
        //    {
        //        return false;
        //    }

        //    string subject = EmailConstants.SUBJECT_APPROVE_EXTENSION;
        //    string content = EmailConstants.BodyApproveExtensionEmail(borrowingId, bookId);
        //    string emailTo = borrowingDetail.Borrowing?.Requestor?.Email ?? string.Empty;

        //    await _emailService.SendEmailAsync(emailTo, subject, content);
        //    return true;
        //}

        public async Task<bool> DeleteAsync(long borrowingId, long bookId)
        {
            var borrowingDetail = await _unitOfWork.BorrowingDetailRepository.GetAsync(b => !b.IsDeleted && b.BorrowingId == borrowingId && b.BookId == bookId);
            if (borrowingDetail == null)
            {
                return false;
            }
            _unitOfWork.BorrowingDetailRepository.SoftDelete(borrowingDetail);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<IEnumerable<BorrowingDetailResponse>> GetBorrowingDetailsByBorrowingIdAsync(long borrowingId)
        {
            if (!await SetStatusWhenOverdue())
            {
                return null;
            }

            var borrowingDetails = await _unitOfWork
                .BorrowingDetailRepository
                .GetAllAsync(b => b.BorrowingId == borrowingId && !b.IsDeleted
                , b => b.Borrowing, b => b.Borrowing.Approver, b => b.Borrowing.Requestor, b => b.Book);
            return _mapper.Map<IEnumerable<BorrowingDetailResponse>>(borrowingDetails);
        }

        public async Task<IEnumerable<BorrowingDetailResponse>> GetBorrowingDetailsRequestExtend()
        {
            var borrowingDetails = await _unitOfWork
                .BorrowingDetailRepository
                .GetAllAsync(b => !b.IsDeleted && b.StatusExtend == StatusBorrowingExtend.WAITING
                , b => b.Borrowing.Requestor, b => b.Book);
            return _mapper.Map<IEnumerable<BorrowingDetailResponse>>(borrowingDetails);
        }

        private async Task<bool> SetStatusWhenOverdue()
        {
            var borrowingDetails = await _unitOfWork
                .BorrowingDetailRepository
                .GetAllAsync(b => b.Status == StatusBorrowingDetail.BORROWING && b.ReturnedAt < DateTime.Now);
            if (borrowingDetails == null || !borrowingDetails.Any())
            {
                return true;
            }
            foreach (var item in borrowingDetails)
            {
                item.Status = StatusBorrowingDetail.OVERDUE;
                _unitOfWork.BorrowingDetailRepository.Update(item);
            }
            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
