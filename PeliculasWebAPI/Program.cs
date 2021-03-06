using Microsoft.EntityFrameworkCore;
using PeliculasWebAPI;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(opc => 
                        opc.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDBContext>(
                    opciones => {
                        opciones.UseSqlServer(
                            connectionString, sqlServer => sqlServer.UseNetTopologySuite()
                        );
                        /* Solo lectura, comportamiento del query */
                        opciones.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        /* Configurando Lazy Loading */
                        //opciones.UseLazyLoadingProxies();
                    }
                 );

/* Agregando AutoMapper */
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
