using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SkinCare_Data;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SkincareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors("AllowSpecificOrigin");

// Enable HTTPS redirection
app.UseHttpsRedirection();




// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();


// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();