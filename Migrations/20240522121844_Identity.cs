using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvioIndustrija.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Avion",
            //    columns: table => new
            //    {
            //        AvionID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ImeAviona = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Kompanija = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        GodinaProizvodnje = table.Column<short>(type: "smallint", nullable: false),
            //        Visinam = table.Column<short>(name: "Visina(m)", type: "smallint", nullable: false),
            //        Širinam = table.Column<short>(name: "Širina(m)", type: "smallint", nullable: false),
            //        Dužinam = table.Column<short>(name: "Dužina(m)", type: "smallint", nullable: false),
            //        BrojSjedištaBiznisKlase = table.Column<short>(type: "smallint", nullable: false),
            //        BrojSjedištaEkonomskeKlase = table.Column<short>(type: "smallint", nullable: false),
            //        Nosivostkg = table.Column<short>(name: "Nosivost(kg)", type: "smallint", nullable: false),
            //        KapacitetRezervoaraL = table.Column<short>(name: "KapacitetRezervoara(L)", type: "smallint", nullable: false),
            //        MaksimalniDometkm = table.Column<short>(name: "MaksimalniDomet(km)", type: "smallint", nullable: false),
            //        ImageURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Avion", x => x.AvionID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Putnik",
            //    columns: table => new
            //    {
            //        PutnikId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Ime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Prezime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Pol = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
            //        Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Putnik", x => x.PutnikId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Relacija",
            //    columns: table => new
            //    {
            //        RelacijaID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Gradod = table.Column<string>(name: "Grad(od)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Državaod = table.Column<string>(name: "Država(od)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Aerodromod = table.Column<string>(name: "Aerodrom(od)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Graddo = table.Column<string>(name: "Grad(do)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Državado = table.Column<string>(name: "Država(do)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Aerodromdo = table.Column<string>(name: "Aerodrom(do)", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        Udaljenostkm = table.Column<short>(name: "Udaljenost(km)", type: "smallint", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Relacija", x => x.RelacijaID);
            //    });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "Let",
            //    columns: table => new
            //    {
            //        LetID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BrojLeta = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
            //        AvionID = table.Column<int>(type: "int", nullable: false),
            //        RelacijaID = table.Column<int>(type: "int", nullable: false),
            //        VrijemePoletanja = table.Column<DateTime>(type: "datetime", nullable: false),
            //        VrijemeSletanja = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Let", x => x.LetID);
            //        table.ForeignKey(
            //            name: "FK_Let_Avion",
            //            column: x => x.AvionID,
            //            principalTable: "Avion",
            //            principalColumn: "AvionID");
            //        table.ForeignKey(
            //            name: "FK_Let_Relacija",
            //            column: x => x.RelacijaID,
            //            principalTable: "Relacija",
            //            principalColumn: "RelacijaID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "IstorijaLetovaPutnika",
            //    columns: table => new
            //    {
            //        PutnikID = table.Column<int>(type: "int", nullable: false),
            //        LetID = table.Column<int>(type: "int", nullable: false),
            //        RedniBrojSjedišta = table.Column<short>(type: "smallint", nullable: false),
            //        Klasa = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
            //        RučniPrtljag8kg = table.Column<string>(name: "RučniPrtljag(<8kg)", type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true, defaultValueSql: "('NE')"),
            //        Koferikg = table.Column<byte>(name: "Koferi(kg)", type: "tinyint", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_IstorijaLetovaPutnika", x => new { x.PutnikID, x.LetID });
            //        table.ForeignKey(
            //            name: "FK_IstorijaLetovaPutnika_Let",
            //            column: x => x.LetID,
            //            principalTable: "Let",
            //            principalColumn: "LetID");
            //        table.ForeignKey(
            //            name: "FK_IstorijaLetovaPutnika_Putnik",
            //            column: x => x.PutnikID,
            //            principalTable: "Putnik",
            //            principalColumn: "PutnikId");
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Avion_GodinaProizvodnje",
            //    table: "Avion",
            //    column: "GodinaProizvodnje");

            //migrationBuilder.CreateIndex(
            //    name: "IX_IstorijaLetovaPutnika_LetID",
            //    table: "IstorijaLetovaPutnika",
            //    column: "LetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Let_AvionID",
            //    table: "Let",
            //    column: "AvionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Let_RelacijaID",
            //    table: "Let",
            //    column: "RelacijaID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Let_VrijemePoletanja",
            //    table: "Let",
            //    column: "VrijemePoletanja");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Putnik_Email",
            //    table: "Putnik",
            //    column: "Email",
            //    unique: true,
            //    filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            //migrationBuilder.DropTable(
            //    name: "IstorijaLetovaPutnika");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "Let");

            //migrationBuilder.DropTable(
            //    name: "Putnik");

            //migrationBuilder.DropTable(
            //    name: "Avion");

            //migrationBuilder.DropTable(
            //    name: "Relacija");
        }
    }
}
