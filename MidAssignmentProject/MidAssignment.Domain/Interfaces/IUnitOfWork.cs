namespace MidAssignment.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IBookRepository BookRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        IRatingRepository RatingRepository { get; }
        IBorrowingRepository BorrowingRepository { get; }
        IBorrowingDetailRepository BorrowingDetailRepository { get; }
        ICartRepository CartRepository { get; }

        Task<int> CommitAsync();

        int Commit();
    }
}
