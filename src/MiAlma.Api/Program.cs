using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register MediatR handlers from Application assembly
builder.Services.AddMediatR(typeof(MiAlma.Application.DTOs.RfpDto).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
