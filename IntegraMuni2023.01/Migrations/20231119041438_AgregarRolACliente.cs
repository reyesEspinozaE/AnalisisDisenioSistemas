using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegraMuni2023._01.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRolACliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    Identificacion = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Clave = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__71ABD0A75F8D6E01", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    EmpleadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Rol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Empleado__958BE6F0A583FEAB", x => x.EmpleadoID);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    NoticiaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TituloNoticia = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    Desarrollo = table.Column<string>(type: "text", nullable: false),
                    NivelPrioridad = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proyecto__F33000EF6FEC6216", x => x.NoticiaID);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    ServicioID = table.Column<int>(type: "int", nullable: false),
                    NombreServicio = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Tarifa = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TipoTarifa = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Servicio__D5AEEC225AA6B496", x => x.ServicioID);
                });

            migrationBuilder.CreateTable(
                name: "Tramites",
                columns: table => new
                {
                    TramiteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFinalizacion = table.Column<DateOnly>(type: "date", nullable: false),
                    PagoPendiente = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    FormularioPagoCompletado = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    Identificacion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tramites__BDB793F660E72B4F", x => x.TramiteID);
                    table.ForeignKey(
                        name: "FK__Tramites__Client__267ABA7A",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID");
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Prioridad = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpleadoID = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tareas__5CD836719685CBB5", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK__Tareas__Empleado__35BCFE0A",
                        column: x => x.EmpleadoID,
                        principalTable: "Empleados",
                        principalColumn: "EmpleadoID");
                });

            migrationBuilder.CreateTable(
                name: "ServiciosContratados",
                columns: table => new
                {
                    ServicioContratadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicioID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFinalizacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Consumo = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EstadoServicio = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Servicio__872007F9577521DD", x => x.ServicioContratadoID);
                    table.ForeignKey(
                        name: "FK__Servicios__Clien__2C3393D0",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID");
                    table.ForeignKey(
                        name: "FK__Servicios__Servi__2B3F6F97",
                        column: x => x.ServicioID,
                        principalTable: "Servicios",
                        principalColumn: "ServicioID");
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    PagoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicioContratadoID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    Identificacion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    MontoPago = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaPago = table.Column<DateOnly>(type: "date", nullable: false),
                    MetodoPago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EstadoPago = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos__F00B6158D58C2CA2", x => x.PagoID);
                    table.ForeignKey(
                        name: "FK__Pagos__ClienteID__300424B4",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "ClienteID");
                    table.ForeignKey(
                        name: "FK__Pagos__ServicioC__2F10007B",
                        column: x => x.ServicioContratadoID,
                        principalTable: "ServiciosContratados",
                        principalColumn: "ServicioContratadoID");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Empleado__531402F3414950AE",
                table: "Empleados",
                column: "CorreoElectronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ClienteID",
                table: "Pagos",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ServicioContratadoID",
                table: "Pagos",
                column: "ServicioContratadoID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosContratados_ClienteID",
                table: "ServiciosContratados",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosContratados_ServicioID",
                table: "ServiciosContratados",
                column: "ServicioID");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_EmpleadoID",
                table: "Tareas",
                column: "EmpleadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_ClienteID",
                table: "Tramites",
                column: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Tramites");

            migrationBuilder.DropTable(
                name: "ServiciosContratados");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Servicios");
        }
    }
}
