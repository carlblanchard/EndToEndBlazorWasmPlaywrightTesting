using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[TestFixture]
public class BlazorTest : PageTest
{
    private static WebApplication _app = null!;

    [OneTimeSetUp]
    public static void AssemblyInitialize2()
    {
        //Create the API server and pass the httpClient to the front end
        var apiClient = new WebApplicationFactory<WebApi.WeatherForecast>();
        var apiHttpClient = apiClient.CreateClient();

        Console.Error.WriteLine("AssemblyInitialize");
        var baseDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NunitTests");
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions(){
            EnvironmentName = "Development",
            ContentRootPath = baseDir,
            WebRootPath = Path.Combine(baseDir, "wwwroot"),
            ApplicationName = "BlazorWasmApp",
        });

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<HttpClient>(apiHttpClient);

        _app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!_app.Environment.IsDevelopment())
        {
            _app.UseExceptionHandler("/Error");
        }


        _app.UseStaticFiles();

        _app.UseRouting();

        _app.MapBlazorHub();
        
        //Balzor Server
        //_app.MapFallbackToPage("/_Host");

        var readyTcs = new CancellationTokenSource();
        _ = Task.Run(async() =>
        {
            await _app.StartAsync(readyTcs.Token);
            readyTcs.Cancel();
        }).ConfigureAwait(false);
        readyTcs.Token.WaitHandle.WaitOne();
    }

    [OneTimeTearDown]
    public async static Task AssemblyCleanup()
    {
        await _app.StopAsync();
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        var options = base.ContextOptions() ?? new();
        options.BaseURL = "http://localhost:5000";
        return options;
    }
}