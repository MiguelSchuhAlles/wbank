using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Banking.Infrastructure.Migrations
{
    public partial class CreatedTransferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    SameTitularity = table.Column<bool>(nullable: false),
                    RecipientName = table.Column<string>(nullable: false),
                    RecipientCode = table.Column<string>(nullable: false),
                    RecipientAccountCode = table.Column<string>(nullable: false),
                    RecipientBranchNumber = table.Column<int>(nullable: false),
                    RecipientBankNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transferences_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transferences_AccountId",
                table: "Transferences",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferences_Status_AccountId",
                table: "Transferences",
                columns: new[] { "Status", "AccountId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transferences");
        }
    }
}
