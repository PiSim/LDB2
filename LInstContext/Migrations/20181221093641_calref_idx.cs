using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LInst.Migrations
{
    public partial class calref_idx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalibrationResults",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationResults", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentUtilizationAreas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentUtilizationAreas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersonRoles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CalibrationDueDate = table.Column<DateTime>(nullable: true),
                    CalibrationInterval = table.Column<int>(nullable: true),
                    CalibrationResponsibleID = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InstrumentTypeID = table.Column<int>(nullable: false),
                    IsInService = table.Column<bool>(nullable: false, defaultValue: true),
                    IsUnderControl = table.Column<bool>(nullable: false, defaultValue: false),
                    ManufacturerID = table.Column<int>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    SupplierID = table.Column<int>(nullable: true),
                    UtilizationAreaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Instruments_Organizations_CalibrationResponsibleID",
                        column: x => x.CalibrationResponsibleID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentTypes_InstrumentTypeID",
                        column: x => x.InstrumentTypeID,
                        principalTable: "InstrumentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instruments_Organizations_ManufacturerID",
                        column: x => x.ManufacturerID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Instruments_Organizations_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentUtilizationAreas_UtilizationAreaID",
                        column: x => x.UtilizationAreaID,
                        principalTable: "InstrumentUtilizationAreas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoleMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsSelected = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    OrganizationRoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRoleMappings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrganizationRoleMappings_Organizations_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationRoleMappings_OrganizationRoles_OrganizationRoleID",
                        column: x => x.OrganizationRoleID,
                        principalTable: "OrganizationRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    PersonID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonRoleMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsSelected = table.Column<bool>(nullable: false),
                    PersonID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRoleMappings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonRoleMappings_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRoleMappings_PersonRoles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "PersonRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CalibrationResultID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false),
                    LaboratoryID = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TechID = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CalibrationReports_CalibrationResults_CalibrationResultID",
                        column: x => x.CalibrationResultID,
                        principalTable: "CalibrationResults",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationReports_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationReports_Organizations_LaboratoryID",
                        column: x => x.LaboratoryID,
                        principalTable: "Organizations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationReports_People_TechID",
                        column: x => x.TechID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InstrumentID = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InstrumentFiles_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentMaintenanceEvents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    InstrumentID = table.Column<int>(nullable: false),
                    TechID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentMaintenanceEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InstrumentMaintenanceEvents_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstrumentMaintenanceEvents_People_TechID",
                        column: x => x.TechID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentProperties",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InstrumentID = table.Column<int>(nullable: false),
                    IsCalibrationProperty = table.Column<bool>(nullable: false, defaultValue: false),
                    LowerLimit = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TargetValue = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    UpperLimit = table.Column<double>(nullable: false),
                    Value = table.Column<double>(nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentProperties", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InstrumentProperties_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsSelected = table.Column<bool>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    UserRoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMappings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_UserRoles_UserRoleID",
                        column: x => x.UserRoleID,
                        principalTable: "UserRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CalibrationReportID = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CalibrationFiles_CalibrationReports_CalibrationReportID",
                        column: x => x.CalibrationReportID,
                        principalTable: "CalibrationReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationReportReferences",
                columns: table => new
                {
                    CalibrationReportID = table.Column<int>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationReportReferences", x => new { x.CalibrationReportID, x.InstrumentID });
                    table.ForeignKey(
                        name: "FK_CalibrationReportReference_CalRep_CalRepID",
                        column: x => x.CalibrationReportID,
                        principalTable: "CalibrationReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationReportReferences_Instruments_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instruments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationReportProperties",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CalibrationReportID = table.Column<int>(nullable: false),
                    LowerLimit = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentPropertyID = table.Column<int>(nullable: true),
                    TargetValue = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    UpperLimit = table.Column<double>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationReportProperties", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CalibrationReportProperties_CalibrationReports_CalibrationRe~",
                        column: x => x.CalibrationReportID,
                        principalTable: "CalibrationReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationReportProperties_InstrumentProperties_ParentPrope~",
                        column: x => x.ParentPropertyID,
                        principalTable: "InstrumentProperties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFiles_CalibrationReportID",
                table: "CalibrationFiles",
                column: "CalibrationReportID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReportProperties_CalibrationReportID",
                table: "CalibrationReportProperties",
                column: "CalibrationReportID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReportProperties_ParentPropertyID",
                table: "CalibrationReportProperties",
                column: "ParentPropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReportReferences_InstrumentID",
                table: "CalibrationReportReferences",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReports_CalibrationResultID",
                table: "CalibrationReports",
                column: "CalibrationResultID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReports_InstrumentID",
                table: "CalibrationReports",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReports_LaboratoryID",
                table: "CalibrationReports",
                column: "LaboratoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationReports_TechID",
                table: "CalibrationReports",
                column: "TechID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentFiles_InstrumentID",
                table: "InstrumentFiles",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentMaintenanceEvents_InstrumentID",
                table: "InstrumentMaintenanceEvents",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentMaintenanceEvents_TechID",
                table: "InstrumentMaintenanceEvents",
                column: "TechID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProperties_InstrumentID",
                table: "InstrumentProperties",
                column: "InstrumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_CalibrationResponsibleID",
                table: "Instruments",
                column: "CalibrationResponsibleID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_Code",
                table: "Instruments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_InstrumentTypeID",
                table: "Instruments",
                column: "InstrumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_ManufacturerID",
                table: "Instruments",
                column: "ManufacturerID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_SupplierID",
                table: "Instruments",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_UtilizationAreaID",
                table: "Instruments",
                column: "UtilizationAreaID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRoleMappings_OrganizationID",
                table: "OrganizationRoleMappings",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRoleMappings_OrganizationRoleID",
                table: "OrganizationRoleMappings",
                column: "OrganizationRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRoleMappings_PersonID",
                table: "PersonRoleMappings",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRoleMappings_RoleID",
                table: "PersonRoleMappings",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_UserID",
                table: "UserRoleMappings",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_UserRoleID",
                table: "UserRoleMappings",
                column: "UserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonID",
                table: "Users",
                column: "PersonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationFiles");

            migrationBuilder.DropTable(
                name: "CalibrationReportProperties");

            migrationBuilder.DropTable(
                name: "CalibrationReportReferences");

            migrationBuilder.DropTable(
                name: "InstrumentFiles");

            migrationBuilder.DropTable(
                name: "InstrumentMaintenanceEvents");

            migrationBuilder.DropTable(
                name: "OrganizationRoleMappings");

            migrationBuilder.DropTable(
                name: "PersonRoleMappings");

            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "InstrumentProperties");

            migrationBuilder.DropTable(
                name: "CalibrationReports");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropTable(
                name: "PersonRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CalibrationResults");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "InstrumentTypes");

            migrationBuilder.DropTable(
                name: "InstrumentUtilizationAreas");
        }
    }
}
