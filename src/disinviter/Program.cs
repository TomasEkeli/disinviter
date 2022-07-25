using disinviter;
using disinviter.Commands;
using Dolittle.SDK;
using Dolittle.SDK.Extensions.AspNet;
using Microsoft.AspNetCore.ResponseCompression;

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

services.AddHttpContextAccessor();
services.AddRazorPages();
services.AddServerSideBlazor();
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

SingleTenant.TenantId = app.Configuration["SINGLE_TENANT_ID"];
app.UseDolittle();
app.UseCors("AllowAnyGet");

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
app.MapHub<PersonCommands>("/person");
app.MapHub<ChatHub>("/chatHub");
app.MapFallbackToPage("/_Host");

app.Run();
