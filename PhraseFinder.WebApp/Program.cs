using PhraseFinder.WebApp.Options;
using PhraseFinderServiceReference;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();

// Configuration
builder.Services.Configure<TextValidationOptions>(
    builder.Configuration.GetSection("Validation:Text"));
builder.Services.Configure<TextFileValidationOptions>(
    builder.Configuration.GetSection("Validation:TextFile"));

// PhraseFinderService
//builder.Services.AddSingleton<IPhraseFinderService, PhraseFinderServiceClient>();
builder.Services.AddSingleton<IPhraseFinderService, PhraseFinderServiceDev>();

builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();

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

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
