using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MoneyRate.Data;
using Radzen;
using MoneyRate.Services;
using MoneyRate.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddRadzenComponents();
builder.Services.AddSingleton<ICurrencyService, CurrencyService>(); 

builder.Services.AddHttpClient<ICurrencyApiClient, ExchangeRateApiClient>();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

builder.Services.Configure<CurrencyApiSettings>(
    builder.Configuration.GetSection("CurrencyApi"));

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
