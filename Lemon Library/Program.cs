using System.Text;
using Application.Interfaces;
using Application.Services;
using Application.SwaggerFilters;
using Business.Helpers;
using Database.Data;
using Database.Entities;
using Database.Repo.Interfaces;
using Database.Repo.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure the database connection

builder.Services.AddDbContext<LibraryContext>(config =>
    config.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
//
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.RequireHttpsMetadata = false;
//         options.SaveToken = true;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(key),
//             ValidateIssuer = true,
//             ValidIssuer = jwtSettings["Issuer"],
//             ValidateAudience = true,
//             ValidAudience = jwtSettings["Audience"],
//         };
//     });
//

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
//
//     // Add the custom filter to handle file uploads
//     c.OperationFilter<FileUploadOperation>();
//     
//     // c.OperationFilter<AddFileUploadConsumesAttribute>();
// });

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//DI
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
builder.Services.AddScoped(typeof(IAuthorRepository), typeof(AuthorRepository));


builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();


var app = builder.Build();

app.UseCors("AllowOrigin");

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
