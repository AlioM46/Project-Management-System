using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Management_System;
using Project_Management_System.Data;
using Project_Management_System.Interfaces;
using Project_Management_System.Options;
using Project_Management_System.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- JWT Config ---------------------- //
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));

// ---------------------- DB Config ---------------------- //
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// ---------------------- Authorization Policies ---------------------- //
builder.Services.AddAuthorization(options =>
{
    // Admin or ProjectManager
    options.AddPolicy("AdminOrManager", policy =>
    {
        policy.RequireRole(enums.enRoles.Admin.ToString(), enums.enRoles.ProjectManager.ToString());
    });

    // Admin, ProjectManager, or Employee
    options.AddPolicy("EmployeeAndAbove", policy =>
    {
        policy.RequireRole(
            enums.enRoles.Admin.ToString(),
            enums.enRoles.ProjectManager.ToString(),
            enums.enRoles.Employee.ToString()
        );
    });
});

// ---------------------- JWT Authentication ---------------------- //
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero, // strict expiration

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };
    });

// ---------------------- Services ---------------------- //
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<ITask, TaskService>();
builder.Services.AddScoped<IProject, ProjectService>();
builder.Services.AddScoped<IReport, ReportService>();
builder.Services.AddScoped<IApplication, ApplicationService>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IEmail, EmailService>();
builder.Services.AddScoped<IAuth, AuthService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ---------------------- Middleware Pipeline ---------------------- //
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// ✅ Authentication MUST come BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
