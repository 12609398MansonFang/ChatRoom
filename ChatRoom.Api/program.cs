using ChatRoom.Api.Data;
using Microsoft.EntityFrameworkCore;
using ChatRoom.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

var connString = builder.Configuration.GetConnectionString("ChatRoom");
builder.Services.AddSqlite<ChatRoomContext>(connString);

var app = builder.Build();

app.UseCors("AllowVueApp");

app.MapMessageEndpoints();
app.MapUserEndpoints();
app.MigrateDb();

app.Run();
