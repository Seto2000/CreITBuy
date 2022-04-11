using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreITBuy.Infrastructures.Migrations
{
    public partial class UpdatingJobRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobRequests_AspNetUsers_UserId",
                table: "UserJobRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId",
                table: "UserJobRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJobRequests",
                table: "UserJobRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserJobRequests_UserId",
                table: "UserJobRequests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserJobRequests",
                newName: "FromUserId");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserJobRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobRequestId1",
                table: "UserJobRequests",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JobRequests",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                table: "JobRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJobRequests",
                table: "UserJobRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRequests_FromUserId",
                table: "UserJobRequests",
                column: "FromUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRequests_JobRequestId",
                table: "UserJobRequests",
                column: "JobRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRequests_JobRequestId1",
                table: "UserJobRequests",
                column: "JobRequestId1",
                unique: true,
                filter: "[JobRequestId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ToUserId",
                table: "JobRequests",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_AspNetUsers_ToUserId",
                table: "JobRequests",
                column: "ToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobRequests_AspNetUsers_FromUserId",
                table: "UserJobRequests",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId",
                table: "UserJobRequests",
                column: "JobRequestId",
                principalTable: "JobRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId1",
                table: "UserJobRequests",
                column: "JobRequestId1",
                principalTable: "JobRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_AspNetUsers_ToUserId",
                table: "JobRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobRequests_AspNetUsers_FromUserId",
                table: "UserJobRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId",
                table: "UserJobRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId1",
                table: "UserJobRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJobRequests",
                table: "UserJobRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserJobRequests_FromUserId",
                table: "UserJobRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserJobRequests_JobRequestId",
                table: "UserJobRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserJobRequests_JobRequestId1",
                table: "UserJobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ToUserId",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserJobRequests");

            migrationBuilder.DropColumn(
                name: "JobRequestId1",
                table: "UserJobRequests");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "JobRequests");

            migrationBuilder.RenameColumn(
                name: "FromUserId",
                table: "UserJobRequests",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "JobRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJobRequests",
                table: "UserJobRequests",
                columns: new[] { "JobRequestId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobRequests_UserId",
                table: "UserJobRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobRequests_AspNetUsers_UserId",
                table: "UserJobRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobRequests_JobRequests_JobRequestId",
                table: "UserJobRequests",
                column: "JobRequestId",
                principalTable: "JobRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
