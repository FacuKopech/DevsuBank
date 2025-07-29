using BankApi.Commands.Clientes;
using BankApi.Commands.Movimientos;
using BankApi.Commands.Cuentas;
using BankApi.Services.Cliente;
using BankApi.Services.Cuenta;
using Data; 
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IClienteValidator, ClienteValidator>();
builder.Services.AddScoped<ICuentaValidator, CuentaValidator>();
builder.Services.AddScoped<ICreateTransactionCommands, CreateTransactionCommandHandler>();
builder.Services.AddScoped<IUpdateTransactionCommands, UpdateTransactionCommandHandler>();
builder.Services.AddScoped<IDeleteTransactionCommands, DeleteTransactionCommandHandler>();
builder.Services.AddScoped<ICreateClientCommand, CreateClientCommandHandler>();
builder.Services.AddScoped<IUpdateClientCommand, UpdateClientCommandHandler>();
builder.Services.AddScoped<IDeleteClientCommand, DeleteClientCommandHandler>();
builder.Services.AddScoped<ICreateAccountCommand, CreateAccountCommandHandler>();
builder.Services.AddScoped<IUpdateAccountCommand, UpdateAccountCommandHandler>();
builder.Services.AddScoped<IDeleteAccountCommand, DeleteAccountCommandHandler>();
builder.Services.AddScoped<PasswordHasher<Cliente>>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Data")
    ));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
