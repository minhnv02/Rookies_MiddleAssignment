using Microsoft.EntityFrameworkCore;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using Microsoft.Extensions.Configuration;


namespace MidAssignment.Infrastructure.DataAccess
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() { }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<BorrowingDetail> BorrowingDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity properties and relationships
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasDefaultValue(StatusUsersConstants.IN_ACTIVE);
                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TokenHash).IsRequired();
                entity.Property(e => e.TokenSalt).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ReleaseYear).IsRequired();
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<Borrowing>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RequestorId).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasDefaultValue(StatusBorrowing.WAITING);

                entity.HasOne(b => b.Requestor)
                .WithMany()
                .HasForeignKey(b => b.RequestorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Approver)
                .WithMany()
                .HasForeignKey(b => b.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BorrowingDetail>(entity =>
            {
                entity.HasKey(e => new { e.BorrowingId, e.BookId });
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ReturnedAt).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.Borrowing)
                      .WithMany(b => b.BorrowingDetails)
                      .HasForeignKey(e => e.BorrowingId);
                entity.HasOne(e => e.Book)
                      .WithMany(b => b.BorrowingDetails)
                      .HasForeignKey(e => e.BookId);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.UserId });
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Book)
                      .WithMany(b => b.Comments)
                      .HasForeignKey(e => e.BookId);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.UserId });
                entity.Property(e => e.Rate).IsRequired().HasDefaultValue(1);
                entity.HasOne(e => e.Book)
                      .WithMany(b => b.Ratings)
                      .HasForeignKey(e => e.BookId);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Ratings)
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}
