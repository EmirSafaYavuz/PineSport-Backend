using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SchoolExtendedWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Schools_SchoolId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_StudentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Users_ManagerId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTrainer_Users_TrainersUserId",
                table: "SessionTrainer");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgress_Users_StudentId",
                table: "StudentProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSession_Users_StudentId",
                table: "StudentSession");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BranchId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ParentId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_ManagerId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Schools");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Schools",
                newName: "SchoolPhone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Schools",
                newName: "SchoolName");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Schools",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Schools",
                newName: "SchoolAddress");

            migrationBuilder.AddColumn<int>(
                name: "SchoolUserId",
                table: "Branches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Specialization = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Students_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_SchoolUserId",
                table: "Branches",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_BranchId",
                table: "Students",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Schools_SchoolId",
                table: "Branches",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Schools_SchoolUserId",
                table: "Branches",
                column: "SchoolUserId",
                principalTable: "Schools",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Users_UserId",
                table: "Schools",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTrainer_Trainers_TrainersUserId",
                table: "SessionTrainer",
                column: "TrainersUserId",
                principalTable: "Trainers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgress_Students_StudentId",
                table: "StudentProgress",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSession_Students_StudentId",
                table: "StudentSession",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Schools_SchoolId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Schools_SchoolUserId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Users_UserId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTrainer_Trainers_TrainersUserId",
                table: "SessionTrainer");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgress_Students_StudentId",
                table: "StudentProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSession_Students_StudentId",
                table: "StudentSession");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Branches_SchoolUserId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "SchoolUserId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "SchoolPhone",
                table: "Schools",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "SchoolName",
                table: "Schools",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SchoolAddress",
                table: "Schools",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Schools",
                newName: "ManagerId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Schools",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchId",
                table: "Users",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParentId",
                table: "Users",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_ManagerId",
                table: "Schools",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Schools_SchoolId",
                table: "Branches",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_StudentId",
                table: "Payments",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Users_ManagerId",
                table: "Schools",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTrainer_Users_TrainersUserId",
                table: "SessionTrainer",
                column: "TrainersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgress_Users_StudentId",
                table: "StudentProgress",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSession_Users_StudentId",
                table: "StudentSession",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
