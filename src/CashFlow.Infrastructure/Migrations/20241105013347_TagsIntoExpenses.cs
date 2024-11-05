using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TagsIntoExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // O código abaixo foi criado automaticamente, pois na migration
            // anterior a tabela User foi renomeada manualmente como Users
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Expenses_User_UserId",
            //    table: "Expenses");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_User",
            //    table: "User");

            //migrationBuilder.RenameTable(
            //    name: "User",
            //    newName: "Users");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Users",
            //    table: "Users",
            //    column: "Id");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ExpenseId",
                table: "Tags",
                column: "ExpenseId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Expenses_Users_UserId",
            //    table: "Expenses",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
