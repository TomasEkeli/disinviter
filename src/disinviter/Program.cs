using disinviter;
using disinviter.Commands;
using disinviter.Data;
using Dolittle.SDK;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

// todo: set up cors for just the current domain (env variable?)
services.AddCors(cors =>
    {
        cors.AddPolicy(
            "AllowAnyGet",
            policy => policy
                .AllowAnyOrigin()
                .WithMethods("GET")
                .AllowAnyHeader()
        );
    }
);

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddSingleton<WeatherForecastService>();
services.AddDolittle();

services.AddResponseCompression(options =>
    {
        options.MimeTypes = ResponseCompressionDefaults
            .MimeTypes
            .Concat(
                new[] { "application/octet-stream" }
            );
    }
);
services.AddSignalR();

var app = builder.Build();

app.UseCors("AllowAnyGet");

app.RegisterApplicationLifetimeEvents();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chatHub");
app.MapFallbackToPage("/_Host");

app.Run();
