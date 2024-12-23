using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class ModelContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CREATEDATE = table.Column<DateTime>(type: "DATE", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    IMAGELINK = table.Column<string>(type: "VARCHAR2(300)", unicode: false, maxLength: 300, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: true),
                    ACCOUNTPRIVILEGE = table.Column<string>(type: "VARCHAR2(1)", unicode: false, maxLength: 1, nullable: false, defaultValueSql: "'n' "),
                    PasswordHash = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "VARCHAR2(256)", unicode: false, maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "VARCHAR2(256)", unicode: false, maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKACCOUNT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AUTHOR",
                columns: table => new
                {
                    ID = table.Column<short>(type: "NUMBER(5)", precision: 5, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(30)", unicode: false, maxLength: 30, nullable: false),
                    IMAGE = table.Column<string>(type: "VARCHAR2(300)", unicode: false, maxLength: 300, nullable: true),
                    WIKIPEDIALINK = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKAUTHOR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BADGE",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    BACKGROUNDCOLOR = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "'ffffff' "),
                    NAMECOLOR = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "'000000' "),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKBADGE", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "CHARACTER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(12)", precision: 12, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(30)", unicode: false, maxLength: 30, nullable: false),
                    IMAGE = table.Column<string>(type: "VARCHAR2(300)", unicode: false, maxLength: 300, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKCHARACTER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GENRE",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "VARCHAR2(24)", unicode: false, maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKGENRE", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "MEDIUM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "VARCHAR2(30)", unicode: false, maxLength: 30, nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR2(14)", unicode: false, maxLength: 14, nullable: true, defaultValueSql: "'Not finished'\n    "),
                    COUNT = table.Column<byte>(type: "NUMBER(4)", precision: 4, nullable: false, defaultValueSql: "0 "),
                    POSTER = table.Column<string>(type: "VARCHAR2(300)", unicode: false, maxLength: 300, nullable: true),
                    PUBLISHDATE = table.Column<DateTime>(type: "DATE", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(1000)", unicode: false, maxLength: 1000, nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(1)", unicode: false, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMEDIUM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "STUDIO",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    WIKIPEDIALINK = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKSTUDIO", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_ACCOUNT_UserId",
                        column: x => x.UserId,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_ACCOUNT_UserId",
                        column: x => x.UserId,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_ACCOUNT_UserId",
                        column: x => x.UserId,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FRIEND",
                columns: table => new
                {
                    ACCOUNTID1 = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    ACCOUNTID2 = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKFRIEND", x => new { x.ACCOUNTID1, x.ACCOUNTID2 });
                    table.ForeignKey(
                        name: "FKUSER1",
                        column: x => x.ACCOUNTID1,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKUSER2",
                        column: x => x.ACCOUNTID2,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RoleId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_ACCOUNT_UserId",
                        column: x => x.UserId,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USERBADGE",
                columns: table => new
                {
                    ACCOUNTID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    BADGENAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKUSERBADGE", x => new { x.ACCOUNTID, x.BADGENAME });
                    table.ForeignKey(
                        name: "FKBADGEUSER",
                        column: x => x.BADGENAME,
                        principalTable: "BADGE",
                        principalColumn: "NAME");
                    table.ForeignKey(
                        name: "FKUSERBADGE",
                        column: x => x.ACCOUNTID,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "USERCHARACTER",
                columns: table => new
                {
                    ACCOUNTID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    CHARACTERID = table.Column<long>(type: "NUMBER(12)", precision: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKUSERCHARACTER", x => new { x.ACCOUNTID, x.CHARACTERID });
                    table.ForeignKey(
                        name: "FKCHARACTERUSER",
                        column: x => x.CHARACTERID,
                        principalTable: "CHARACTER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKUSERCHARACTER",
                        column: x => x.ACCOUNTID,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LISTELEMENT",
                columns: table => new
                {
                    ACCOUNTID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    MEDIUMID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    FINISHEDNUMBER = table.Column<byte>(type: "NUMBER(4)", precision: 4, nullable: true, defaultValueSql: "0 "),
                    STATUS = table.Column<string>(type: "VARCHAR2(13)", unicode: false, maxLength: 13, nullable: false, defaultValueSql: "'Watching' "),
                    RATING = table.Column<byte>(type: "NUMBER(2)", precision: 2, nullable: true, defaultValueSql: "NULL "),
                    MEDIUMCOMMENT = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: true),
                    STARTDATE = table.Column<DateTime>(type: "DATE", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    FINISHDATE = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKLISTELEMENT", x => new { x.ACCOUNTID, x.MEDIUMID });
                    table.ForeignKey(
                        name: "FKELEMENTACCOUNTID",
                        column: x => x.ACCOUNTID,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKELEMENTMEDIUM",
                        column: x => x.MEDIUMID,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MANGA",
                columns: table => new
                {
                    MEDIUMID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    TYPE = table.Column<string>(type: "VARCHAR2(10)", unicode: false, maxLength: 10, nullable: true, defaultValueSql: "'Manga' "),
                    AUTHORID = table.Column<short>(type: "NUMBER(5)", precision: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMANGAID", x => x.MEDIUMID);
                    table.ForeignKey(
                        name: "FKAUTHORID",
                        column: x => x.AUTHORID,
                        principalTable: "AUTHOR",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKMANGAID",
                        column: x => x.MEDIUMID,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MEDIUMCHARACTER",
                columns: table => new
                {
                    MEDIUMID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    CHARACTERID = table.Column<long>(type: "NUMBER(12)", precision: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMEDIUMCHARACTER", x => new { x.MEDIUMID, x.CHARACTERID });
                    table.ForeignKey(
                        name: "FKCHARACTERMEDIUM",
                        column: x => x.CHARACTERID,
                        principalTable: "CHARACTER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKMEDIUMCHARACTER",
                        column: x => x.MEDIUMID,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MEDIUMCONNECTION",
                columns: table => new
                {
                    IDMEDIUM1 = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    IDMEDIUM2 = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMEDIUMCONNECTION", x => new { x.IDMEDIUM1, x.IDMEDIUM2 });
                    table.ForeignKey(
                        name: "FKMEDIUMCONNECTION1",
                        column: x => x.IDMEDIUM1,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKMEDIUMCONNECTION2",
                        column: x => x.IDMEDIUM2,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MEDIUMGENRE",
                columns: table => new
                {
                    IDMEDIUM = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    GENRENAME = table.Column<string>(type: "VARCHAR2(24)", unicode: false, maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKMEDIUMGENRE", x => new { x.IDMEDIUM, x.GENRENAME });
                    table.ForeignKey(
                        name: "FKGENREMEDIUM",
                        column: x => x.GENRENAME,
                        principalTable: "GENRE",
                        principalColumn: "NAME");
                    table.ForeignKey(
                        name: "FKMEDIUMGENRE",
                        column: x => x.IDMEDIUM,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "REVIEW",
                columns: table => new
                {
                    ACCOUNTID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    MEDIUMID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: false),
                    FEELING = table.Column<string>(type: "VARCHAR2(17)", unicode: false, maxLength: 17, nullable: false, defaultValueSql: "'Recommended' "),
                    POSTDATE = table.Column<DateTime>(type: "DATE", nullable: false, defaultValueSql: "CURRENT_DATE ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKREVIEW", x => new { x.ACCOUNTID, x.MEDIUMID });
                    table.ForeignKey(
                        name: "FKREVIEWMEDIUM",
                        column: x => x.MEDIUMID,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKUSERREVIEW",
                        column: x => x.ACCOUNTID,
                        principalTable: "ACCOUNT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ANIME",
                columns: table => new
                {
                    MEDIUMID = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false),
                    TYPE = table.Column<string>(type: "VARCHAR2(5)", unicode: false, maxLength: 5, nullable: true, defaultValueSql: "'TV'"),
                    STUDIONAME = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKANIMEID", x => x.MEDIUMID);
                    table.ForeignKey(
                        name: "FKANIMEID",
                        column: x => x.MEDIUMID,
                        principalTable: "MEDIUM",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FKNAMESTUDIO",
                        column: x => x.STUDIONAME,
                        principalTable: "STUDIO",
                        principalColumn: "NAME");
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ACCOUNT",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "SYS_C00191762",
                table: "ACCOUNT",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "SYS_C00191763",
                table: "ACCOUNT",
                column: "UserName",
                unique: true,
                filter: "\"UserName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ACCOUNT",
                column: "NormalizedUserName",
                unique: true,
                filter: "\"NormalizedUserName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ANIME_STUDIONAME",
                table: "ANIME",
                column: "STUDIONAME");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "\"NormalizedName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FRIEND_ACCOUNTID2",
                table: "FRIEND",
                column: "ACCOUNTID2");

            migrationBuilder.CreateIndex(
                name: "IX_LISTELEMENT_MEDIUMID",
                table: "LISTELEMENT",
                column: "MEDIUMID");

            migrationBuilder.CreateIndex(
                name: "IX_MANGA_AUTHORID",
                table: "MANGA",
                column: "AUTHORID");

            migrationBuilder.CreateIndex(
                name: "IX_MEDIUMCHARACTER_CHARACTERID",
                table: "MEDIUMCHARACTER",
                column: "CHARACTERID");

            migrationBuilder.CreateIndex(
                name: "IX_MEDIUMCONNECTION_IDMEDIUM2",
                table: "MEDIUMCONNECTION",
                column: "IDMEDIUM2");

            migrationBuilder.CreateIndex(
                name: "IX_MEDIUMGENRE_GENRENAME",
                table: "MEDIUMGENRE",
                column: "GENRENAME");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEW_MEDIUMID",
                table: "REVIEW",
                column: "MEDIUMID");

            migrationBuilder.CreateIndex(
                name: "IX_USERBADGE_BADGENAME",
                table: "USERBADGE",
                column: "BADGENAME");

            migrationBuilder.CreateIndex(
                name: "IX_USERCHARACTER_CHARACTERID",
                table: "USERCHARACTER",
                column: "CHARACTERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANIME");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FRIEND");

            migrationBuilder.DropTable(
                name: "LISTELEMENT");

            migrationBuilder.DropTable(
                name: "MANGA");

            migrationBuilder.DropTable(
                name: "MEDIUMCHARACTER");

            migrationBuilder.DropTable(
                name: "MEDIUMCONNECTION");

            migrationBuilder.DropTable(
                name: "MEDIUMGENRE");

            migrationBuilder.DropTable(
                name: "REVIEW");

            migrationBuilder.DropTable(
                name: "USERBADGE");

            migrationBuilder.DropTable(
                name: "USERCHARACTER");

            migrationBuilder.DropTable(
                name: "STUDIO");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AUTHOR");

            migrationBuilder.DropTable(
                name: "GENRE");

            migrationBuilder.DropTable(
                name: "MEDIUM");

            migrationBuilder.DropTable(
                name: "BADGE");

            migrationBuilder.DropTable(
                name: "CHARACTER");

            migrationBuilder.DropTable(
                name: "ACCOUNT");
        }
    }
}
