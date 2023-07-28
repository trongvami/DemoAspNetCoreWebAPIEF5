using DemoFullIdentityEF6.Data;
using DemoFullIdentityEF6.Models;
using DemoFullIdentityEF6.Repositories;
using DemoFullIdentityEF6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DemoFullIdentityEF6Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("demofullidentityconnect")));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option => {
    option.Password.RequiredLength = 9;
    option.Password.RequiredUniqueChars = 3;
    option.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<DemoFullIdentityEF6Context>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt => opt.SignIn.RequireConfirmedEmail = true);

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => {
    opts.TokenLifespan = TimeSpan.FromMinutes(10);
});
builder.Services.AddCors(option => option.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("DeleteCreateRolePolicy", policy => policy.RequireClaim("Delete Role", "true").RequireClaim("Create Role", "true"));
//    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));
//    options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true"));
//    options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
//    options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("ViewRolePolicy", policy =>
//        policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
//});

builder.Services.AddTransient<IEmailService, EmailRepository>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HIGroceryStore API",
        Version = "v1"
    });

    option.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="bearer"
                }
            },
            new string[]{}
        }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
