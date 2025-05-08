using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using milton.Components;

var builder = WebApplication.CreateBuilder(args);

//SimpleLogger.TargetLogPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
SimpleLogger.TargetLogPath = builder.Environment.WebRootPath;
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ProductSnapshotService>();
builder.Services.AddScoped<SourcesService>();

builder.Services.AddScoped<PriceSnapshotService>();
builder.Services.AddScoped<PriceSourceService>();
builder.Services.AddScoped<ProductService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
