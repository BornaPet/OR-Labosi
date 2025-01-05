using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OtvorenoRacunalstvoLabosi.Controllers.API;
using OtvorenoRacunalstvoLabosi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Mappings));

builder.Services.AddDbContext<OtvorenoRacunarstvoLabosiContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse<string>(null, "An unexpected error occurred", false)));
//    });
//});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Monuments API",
        Version = "v1",
        Description = "API za upravljanje spomenicima",
        Contact = new OpenApiContact
        {
            Name = "Borna Petak",
            Email = "borna.petak@fer.hr",
            Url = new Uri("https://github.com/bornapet"),
        },
        License = new OpenApiLicense
        {
            Name = "Creative Commons Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)",
            Url = new Uri("https://creativecommons.org/licenses/by-sa/4.0/?ref=chooser-v1"),
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monuments API V1");
        c.RoutePrefix = string.Empty;
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
