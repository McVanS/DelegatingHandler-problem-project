using DelegatingHandlerProblem;
using DelegatingHandlerProblem.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();




// Test setup
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddTransient<AuthenticationHeaderHandler>();

builder.Services.AddHttpClient("ConfiguredClient", c =>
{
    c.BaseAddress = new Uri("https://Base");
}).AddHttpMessageHandler<AuthenticationHeaderHandler>();







var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
