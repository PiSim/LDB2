﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LInst.Migrations
{
    public partial class CreateLInstDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalibrationResults",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
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
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InstrumentTypeID = table.Column<int>(nullable: false),
                    SupplierID = table.Column<int>(nullable: true),
                    ManufacturerID = table.Column<int>(nullable: true),
                    CalibrationResponsibleID = table.Column<int>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    UtilizationAreaID = table.Column<int>(nullable: false),
                    IsInService = table.Column<short>(nullable: false),
                    IsUnderControl = table.Column<short>(nullable: false),
                    CalibrationDueDate = table.Column<DateTime>(nullable: true),
                    CalibrationInterval = table.Column<int>(nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoleMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    OrganizationID = table.Column<int>(nullable: false),
                    OrganizationRoleID = table.Column<int>(nullable: false),
                    IsSelected = table.Column<short>(nullable: false)
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
                name: "PersonRoleMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    PersonID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    IsSelected = table.Column<short>(nullable: false)
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
                        .Annotation("MySQL:AutoIncrement", true),
                    CalibrationResultID = table.Column<int>(nullable: false),
                    LaboratoryID = table.Column<int>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false),
                    TechID = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
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
                        .Annotation("MySQL:AutoIncrement", true),
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
                        .Annotation("MySQL:AutoIncrement", true),
                    InstrumentID = table.Column<int>(nullable: false),
                    TechID = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
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
                        .Annotation("MySQL:AutoIncrement", true),
                    InstrumentID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    IsCalibrationProperty = table.Column<short>(nullable: false)
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
                name: "CalibrationFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Path = table.Column<string>(nullable: true),
                    CalibrationReportID = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFiles_CalibrationReportID",
                table: "CalibrationFiles",
                column: "CalibrationReportID");

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
                name: "IX_Instruments_ManufacturerID",
                table: "Instruments",
                column: "ManufacturerID");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_SupplierID",
                table: "Instruments",
                column: "SupplierID");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationFiles");

            migrationBuilder.DropTable(
                name: "InstrumentFiles");

            migrationBuilder.DropTable(
                name: "InstrumentMaintenanceEvents");

            migrationBuilder.DropTable(
                name: "InstrumentProperties");

            migrationBuilder.DropTable(
                name: "InstrumentTypes");

            migrationBuilder.DropTable(
                name: "InstrumentUtilizationAreas");

            migrationBuilder.DropTable(
                name: "OrganizationRoleMappings");

            migrationBuilder.DropTable(
                name: "PersonRoleMappings");

            migrationBuilder.DropTable(
                name: "CalibrationReports");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropTable(
                name: "PersonRoles");

            migrationBuilder.DropTable(
                name: "CalibrationResults");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
