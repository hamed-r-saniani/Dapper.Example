using Microsoft.EntityFrameworkCore.Migrations;

namespace Dapper.Example.Migrations
{
    public partial class StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                 Create PROC usp_GetCompany
                 @CompanyId int
                 AS
                 BEGIN
                 Select * From Companies
                 Where CompanyId = @CompanyId
                 END
                 GO
            ");
            migrationBuilder.Sql(@"
                 Create PROC usp_GetAllCompany
                 AS
                 BEGIN
                 Select * From Companies
                 END
                 GO
            ");
            migrationBuilder.Sql(@"
                 Create PROC usp_AddCompany
                 @CompanyId int OUTPUT,
                 @Name varchar(MAX),
                 @Address varchar(MAX),
                 @City varchar(MAX),
                 @State varchar(MAX),
                 @PostalCode varchar(MAX)
                 AS
                 BEGIN
                 Insert Into Companies(Name,Address,City,State,PostalCode)
                 VALUES(@Name,@Address,@City,@State,@PostalCode);
                 Select @CompanyId = SCOPE_IDENTITY()
                 END
                 GO
            ");
            migrationBuilder.Sql(@"
                 Create PROC usp_UpdateCompany
                 @CompanyId int,
                 @Name varchar(MAX),
                 @Address varchar(MAX),
                 @City varchar(MAX),
                 @State varchar(MAX),
                 @PostalCode varchar(MAX)
                 AS
                 BEGIN
                 UPDATE Companies SET Name = @Name,Address = @Address,City = @City,State = @State,PostalCode = @PostalCode
                 WHERE CompanyId = @CompanyId;
                 END
                 GO
            ");
            migrationBuilder.Sql(@"
                 Create PROC usp_DeleteCompany
                 @CompanyId int
                 AS
                 BEGIN
                 DELETE From Companies WHERE CompanyId = @CompanyId
                 END
                 GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
