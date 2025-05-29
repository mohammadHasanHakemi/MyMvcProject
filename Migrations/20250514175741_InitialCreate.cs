using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMvcProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PosterPath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_persian_ci");

            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Property = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PosterPath = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MusicPath = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    View = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_persian_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musics_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_persian_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_AlbumId",
                table: "Musics",
                column: "AlbumId");
                                migrationBuilder.Sql("ALTER TABLE Albums ENGINE = InnoDB;");
    migrationBuilder.Sql("ALTER TABLE Musics ENGINE = InnoDB;");
     migrationBuilder.Sql("ALTER TABLE Albums CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_persian_ci;");
    migrationBuilder.Sql("ALTER TABLE Musics CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_persian_ci;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musics");

            migrationBuilder.DropTable(
                name: "Albums");
        }
    }
}
