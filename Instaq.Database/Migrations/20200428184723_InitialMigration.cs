using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Instaq.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blacklist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_bin"),
                    reason = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    table = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.id, x.name });
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    photos_count = table.Column<int>(type: "int(11)", nullable: false),
                    feedback_count = table.Column<int>(type: "int(11)", nullable: false),
                    search_count = table.Column<int>(type: "int(11)", nullable: false),
                    infos = table.Column<string>(type: "varchar(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.id, x.customer_id });
                });

            migrationBuilder.CreateTable(
                name: "itags",
                columns: table => new
                {
                    name = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_bin"),
                    posts = table.Column<int>(type: "int(11)", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    refCount = table.Column<int>(type: "int(11)", nullable: false),
                    onBlacklist = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint(20)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    insta_id = table.Column<int>(type: "int(11)", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    slug = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    lat = table.Column<int>(type: "int(11)", nullable: false),
                    lng = table.Column<string>(type: "varchar(11)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    has_public_page = table.Column<bool>(nullable: false),
                    profile_pic_url = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs_feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    customer_id = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    debug_id = table.Column<int>(type: "int(11)", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs_feedback", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs_hashtag_search",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    customer_id = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    data = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs_hashtag_search", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs_upload",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    data = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    customer_id = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs_upload", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mtags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shortcode = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    name = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    score = table.Column<float>(type: "float(11,9)", nullable: false),
                    source = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    onBlacklist = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mtags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "photo_itag_rel",
                columns: table => new
                {
                    shortcode = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    itag = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_bin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.shortcode, x.itag });
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    shortcode = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    largeUrl = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    thumbUrl = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    likes = table.Column<int>(type: "int(11)", nullable: false),
                    comments = table.Column<int>(type: "int(11)", nullable: false),
                    user = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci"),
                    follower = table.Column<int>(type: "int(11)", nullable: false),
                    following = table.Column<int>(type: "int(11)", nullable: false),
                    posts = table.Column<int>(type: "int(11)", nullable: false),
                    location_id = table.Column<long>(type: "bigint(20)", nullable: true),
                    uploaded = table.Column<DateTime>(type: "timestamp", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.shortcode);
                    table.ForeignKey(
                        name: "rel_photos_location",
                        column: x => x.location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "id",
                table: "blacklist",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name",
                table: "blacklist",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "customer_id",
                table: "customer",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "customer",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name",
                table: "itags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id",
                table: "locations",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "feedbackCustomerId",
                table: "logs_feedback",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "logs_feedback",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "hashtagSearchCustomerId",
                table: "logs_hashtag_search",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "logs_hashtag_search",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "debugCustomerId",
                table: "logs_upload",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "logs_upload",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id",
                table: "mtags",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name",
                table: "mtags",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "shortcode",
                table: "mtags",
                column: "shortcode");

            migrationBuilder.CreateIndex(
                name: "itagId",
                table: "photo_itag_rel",
                column: "itag");

            migrationBuilder.CreateIndex(
                name: "shortcode",
                table: "photo_itag_rel",
                column: "shortcode");

            migrationBuilder.CreateIndex(
                name: "created",
                table: "photos",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "rel_photos_location",
                table: "photos",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "imgId",
                table: "photos",
                column: "shortcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "status",
                table: "photos",
                column: "status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blacklist");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "itags");

            migrationBuilder.DropTable(
                name: "logs_feedback");

            migrationBuilder.DropTable(
                name: "logs_hashtag_search");

            migrationBuilder.DropTable(
                name: "logs_upload");

            migrationBuilder.DropTable(
                name: "mtags");

            migrationBuilder.DropTable(
                name: "photo_itag_rel");

            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "locations");
        }
    }
}
