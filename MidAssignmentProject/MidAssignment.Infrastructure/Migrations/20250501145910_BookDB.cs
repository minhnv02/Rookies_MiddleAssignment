using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MidAssignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaysForBorrow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.BookId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    DaysForBorrow = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "InActive"),
                    RoleId = table.Column<byte>(type: "tinyint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Waiting"),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequest_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequest_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequest_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => new { x.BookId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Ratings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequestDetail",
                columns: table => new
                {
                    BorrowingId = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusExtend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequestDetail", x => new { x.BorrowingId, x.BookId });
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetail_BookBorrowingRequest_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "BookBorrowingRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetail_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequest",
                columns: new[] { "Id", "ApproverId", "CreatedAt", "IsDeleted", "RequestorId", "Status", "UserId" },
                values: new object[,]
                {
                    { 1L, "librarian-guid-1", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1", "Approved", null },
                    { 2L, "librarian-guid-1", new DateTime(2023, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-2", "Approved", null },
                    { 3L, "librarian-guid-1", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-3", "Rejected", null },
                    { 4L, "librarian-guid-1", new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1", "Approved", null },
                    { 5L, null, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-4", "Waiting", null },
                    { 6L, null, new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-5", "Waiting", null }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "DaysForBorrow", "Description", "Image", "IsDeleted", "Name", "ReleaseYear" },
                values: new object[,]
                {
                    { 1L, "Harper Lee", 1L, 14, "The unforgettable novel of a childhood in a sleepy Southern town and the crisis of conscience that rocked it.", "tkam.jpg", false, "To Kill a Mockingbird", 1960 },
                    { 2L, "George Orwell", 2L, 14, "A dystopian novel set in a totalitarian state where critical thought is suppressed under a cult of personality.", "1984.jpg", false, "1984", 1949 },
                    { 3L, "F. Scott Fitzgerald", 1L, 10, "A tale of impossible love, dreams, and tragedy set during the Roaring Twenties in America.", "gatsby.jpg", false, "The Great Gatsby", 1925 },
                    { 4L, "Robert C. Martin", 3L, 21, "A handbook of agile software craftsmanship that helps programmers write better code.", "cleancode.jpg", false, "Clean Code", 2008 },
                    { 5L, "Andrew Hunt, David Thomas", 3L, 21, "From journeyman to master - a guide filled with practical advice for software developers.", "pragmatic.jpg", false, "The Pragmatic Programmer", 1999 },
                    { 6L, "Jane Austen", 4L, 14, "A romantic novel of manners that follows the character development of Elizabeth Bennet.", "pride.jpg", false, "Pride and Prejudice", 1813 },
                    { 7L, "Frank Herbert", 5L, 14, "Set in the distant future amidst a feudal interstellar society, Dune tells the story of Paul Atreides.", "dune.jpg", false, "Dune", 1965 },
                    { 8L, "J.R.R. Tolkien", 5L, 14, "A fantasy novel and children's book following the quest of Bilbo Baggins.", "hobbit.jpg", false, "The Hobbit", 1937 },
                    { 9L, "Yuval Noah Harari", 6L, 21, "A book that explores the history of the human species from the emergence of Homo sapiens to the present day.", "sapiens.jpg", false, "Sapiens: A Brief History of Humankind", 2011 },
                    { 10L, "Stephen Hawking", 7L, 14, "Hawking's final book addresses the biggest questions of our time.", "hawking.jpg", false, "Brief Answers to the Big Questions", 2018 },
                    { 11L, "Paulo Coelho", 8L, 10, "A philosophical novel about the journey of a shepherd boy searching for a worldly treasure.", "alchemist.jpg", false, "The Alchemist", 1988 },
                    { 12L, "J.K. Rowling", 5L, 14, "The first novel in the Harry Potter series following a young wizard's adventures at Hogwarts School.", "harrypotter.jpg", false, "Harry Potter and the Philosopher's Stone", 1997 }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "BookId", "UserId", "Author", "DaysForBorrow", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "user-guid-1", "Harper Lee", 14, "tkam.jpg", "To Kill a Mockingbird" },
                    { 3, "user-guid-2", "F. Scott Fitzgerald", 10, "gatsby.jpg", "The Great Gatsby" }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetail",
                columns: new[] { "BookId", "BorrowingId", "CreatedAt", "IsDeleted", "ReturnedAt", "Status", "StatusExtend" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Returned", "" },
                    { 3L, 1L, new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Returned", "" },
                    { 2L, 2L, new DateTime(2023, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowing", "" },
                    { 5L, 2L, new DateTime(2023, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowing", "" },
                    { 7L, 3L, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected", "" },
                    { 9L, 3L, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected", "" },
                    { 4L, 4L, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowing", "" },
                    { 8L, 4L, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowing", "" },
                    { 6L, 5L, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "" },
                    { 11L, 5L, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "" },
                    { 10L, 6L, new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "" },
                    { 12L, 6L, new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "BookId", "Content", "CreatedAt", "IsDeleted", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, "This is a classic that everyone should read at least once. The character development is exceptional.", new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1" },
                    { 2L, 2L, "A haunting vision of a totalitarian future. Still relevant today.", new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-2" },
                    { 3L, 3L, "The symbolism in this book is incredible. Gatsby represents the American dream in all its flawed glory.", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-3" },
                    { 4L, 4L, "This book transformed how I write code. Essential reading for any programmer.", new DateTime(2023, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1" },
                    { 5L, 4L, "I agree! This book should be mandatory in CS courses.", new DateTime(2023, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-4" },
                    { 6L, 5L, "One of the best programming books ever written. Timeless advice.", new DateTime(2023, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-2" },
                    { 7L, 6L, "Jane Austen was ahead of her time. The social commentary is still relevant.", new DateTime(2023, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-3" },
                    { 8L, 7L, "The worldbuilding in this novel is incredible. Herbert created something truly unique.", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-5" },
                    { 9L, 8L, "A perfect introduction to Tolkien's world. I read this to my kids every year.", new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-4" },
                    { 10L, 9L, "This book changed my perspective on human history. Fascinating read!", new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1" },
                    { 11L, 10L, "Hawking makes complex physics accessible to everyone. A brilliant mind.", new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-2" },
                    { 12L, 11L, "A spiritual journey that makes you think about your own path in life.", new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-5" },
                    { 13L, 12L, "The book that sparked a generation's love of reading. Simply magical.", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-3" },
                    { 14L, 2L, "Still incredibly relevant. A classic that seems more prophetic every year.", new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-1" },
                    { 15L, 12L, "My kids love this book as much as I did growing up!", new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "user-guid-4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequest_ApproverId",
                table: "BookBorrowingRequest",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequest_RequestorId",
                table: "BookBorrowingRequest",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequest_UserId",
                table: "BookBorrowingRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetail_BookId",
                table: "BookBorrowingRequestDetail",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BookId",
                table: "Comments",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowingRequestDetail");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequest");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
