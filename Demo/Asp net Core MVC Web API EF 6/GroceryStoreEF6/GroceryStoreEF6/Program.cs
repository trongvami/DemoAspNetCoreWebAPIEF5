using GroceryStoreEF6.Model.ResponseModel;
using GroceryStoreEF6.Persistence;
using GroceryStoreEF6.Repositories;
using GroceryStoreEF6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
//var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
//builder.Services.AddSingleton(emailConfig);
// for entity framework
builder.Services.AddDbContext<GroceryEF6DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));
//builder.Services.AddScoped<IEmailService, EmailRepository>();
// for identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GroceryEF6DbContext>()
    .AddDefaultTokenProviders();
// adding authentication
builder.Services.AddCors(option => option.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddTransient<IEmailService, EmailRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors("MyCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
