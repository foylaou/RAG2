using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAG2.Entities
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                    name: "Users",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        Username = table.Column<string>(type: "varchar(256)", nullable: true), // 指定长度
                        Password = table.Column<string>(type: "varchar(256)", nullable: true), // 指定长度
                        Salt = table.Column<byte[]>(type: "binary(16)", nullable: true), // 已经正确指定长度
                        Email = table.Column<string>(type: "varchar(256)", nullable: true), // 指定长度
                        Sex = table.Column<string>(type: "varchar(50)", nullable: true), // 适当的长度
                        PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true), // 适当的长度
                        HomeNumber = table.Column<string>(type: "varchar(50)", nullable: true), // 适当的长度
                        FirstName = table.Column<string>(type: "varchar(50)", nullable: true), // 适当的长度
                        LastName = table.Column<string>(type: "varchar(50)", nullable: true), // 适当的长度
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Users", x => x.Id);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");

            
            migrationBuilder.CreateTable(
                    name: "message",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        message_content = table.Column<string>(type: "varchar(1024)", nullable: true), // 假设最大长度为1024字符
                        user_id = table.Column<string>(type: "varchar(255)", nullable: true), // 假设用户ID为varchar(255)
                        comefrom_id = table.Column<int>(type: "int", nullable: true),
                        create_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Message", x => x.Id); // 更正主键约束名称
                    })
                .Annotation("MySql:CharSet", "utf8mb4");

        }

    
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
            migrationBuilder.DropTable(
                name: "message");
        }
    }
}

