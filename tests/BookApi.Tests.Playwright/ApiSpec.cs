using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace BookApi.Tests.Functional;

[TestFixture]
public class ApiFunctionalTests
{
    private IPlaywright _pw = null!;
    private IBrowser _browser = null!;
    private readonly string  _baseUrl = Environment.GetEnvironmentVariable("API_URL")! ?? "http://localhost:8080";

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _pw = await Playwright.CreateAsync();
        _browser = await _pw.Chromium.LaunchAsync(new() { Headless = true });
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await _browser.CloseAsync();
        _pw.Dispose();
    }

    [Test]
    public async Task GetBooksReturnOk()
    {
        var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        var fullUrl = $"{_baseUrl}/api/books";
        Console.WriteLine($"Testing endpoint: {fullUrl}");

        var response = await page.APIRequest.GetAsync(fullUrl);

        Assert.Multiple(() =>
        {
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Ok, Is.True);
        });
    }
}