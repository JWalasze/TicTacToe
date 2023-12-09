using Lib;
using Lib.Services;
using Lib.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using webapi.Hubs;
using webapi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration["ConnectionStrings:MyConnectionString"]));
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IRankingService, RankingService>();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(b => b
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    //.AllowAnyOrigin()
    .SetIsOriginAllowed((host) => true)
);
//app.UseRouting();
//app.UseWebSockets();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/moves")
//    {
//        if (context.WebSockets.IsWebSocketRequest)
//        {
//            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//        }
//        else
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;
//        }
//    }
//    else
//    {
//        await next(context);
//    }

//});
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/moves");

app.Run();
