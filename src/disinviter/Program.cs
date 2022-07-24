using disinviter;
using disinviter.Commands;
using disinviter.Data;
using Dolittle.SDK;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDolittle();
builder.Services.AddResponseCompression(options =>
    {
        options.MimeTypes = ResponseCompressionDefaults
            .MimeTypes
            .Concat(
                new[] { "application/octet-stream" }
            );
    }
);

var app = builder.Build();

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
