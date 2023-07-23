using DemoWebAPIEF6HienLTH.Entities;
using DemoWebAPIEF6HienLTH.Repositories;
using DemoWebAPIEF6HienLTH.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyShopHienLTHAspNetCoreEF6Context>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));
//builder.Services.AddCors(p=>p.AddPolicy("MyCors", build => {
//    build.WithOrigins("https://localhost:3000");
//    build.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
//}));
builder.Services.AddCors(option=>option.AddDefaultPolicy(policy=> policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
