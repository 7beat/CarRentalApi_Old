using CarRentalAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDbContext(builder.Configuration);

// Authentication
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

// Repositories
builder.Services.ConfigureRepositories();

// Email sender
builder.Services.ConfigureEmailService();

// Mapper
builder.Services.ConfigureMapping();

// FluentValidation
builder.Services.ConfigureFluentValidation();

var app = builder.Build();

// WIP
await app.SeedIdentityDb(app.Configuration.GetValue<bool>("AutoMigration"));

//await app.AssignVehiclesToUsers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
