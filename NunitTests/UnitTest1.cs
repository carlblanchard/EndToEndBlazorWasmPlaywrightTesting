using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests;

[TestFixture]
public class Nunit_UnitTest1 : BlazorTest
{
    [Test]
    public async Task CountTest()
    {
        await Page.GotoAsync("/");

        await Expect(Page).ToHaveTitleAsync("Index");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Counter" }).ClickAsync();

        for (int i = 0; i < 10; i++)
        {
            await Page.GetByRole(AriaRole.Button, new() { Name = "Click me" }).ClickAsync();
            await Expect(Page.GetByText($"Current count: {i + 1}")).ToBeVisibleAsync();
        }
    }
    [Test]
    public async Task FetchDataTest()
    {
        await Page.GotoAsync("/");

        await Expect(Page).ToHaveTitleAsync("Index");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Fetch data" }).ClickAsync();

        var lastRow = Page.GetByRole(AriaRole.Cell, new() { NameString = "23/11/2022" });

        await Expect(lastRow).ToBeVisibleAsync();


    }
}