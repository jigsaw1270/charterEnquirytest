using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightNUnitTests
{
    public class YachtingDestinationTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage Page;

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            Page = await _browser.NewPageAsync();
        }

        [Test]
        public async Task ShouldNavigateToRandomYachtingDestination()
        {
            // Go to the website
            await Page.GotoAsync("https://web-dev.bluebnc.com/en-us");

            // Wait for the destination section to be visible
            await Page.Locator("h2.title.mark-accent.mt-6.mb-3:has-text('Our Yachting Destinations in the Balearics')")
                .WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });  // 10-second timeout

            // Define the destination cards
            var destinations = new[]
            {
                new { Url = "/en-us/yacht-charter/spain/balearic-islands", Selector = "a.card[href='/en-us/yacht-charter/spain/balearic-islands']" },
                new { Url = "/en-us/yacht-charter/spain/balearic-islands/mallorca", Selector = "a.card[href='/en-us/yacht-charter/spain/balearic-islands/mallorca']" },
                new { Url = "/en-us/yacht-charter/spain/balearic-islands/ibiza", Selector = "a.card[href='/en-us/yacht-charter/spain/balearic-islands/ibiza']" }
            };

            // Randomly select a destination
            var random = new Random();
            var selectedDestination = destinations[random.Next(destinations.Length)];

            // Click on the randomly selected destination
            await Page.Locator(selectedDestination.Selector).ClickAsync();

            // Wait for the navigation to complete
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 10000 });

 await Expect(Page).ToHaveURLAsync(new Regex(selectedDestination.Url), new() { Timeout = 5000 });
            // Assert that the URL matches the expected destination URL
            // try
            // {
             
            //     Console.WriteLine($"Successfully navigated to: {Page.Url}");
            // }
            // catch (PlaywrightException ex)
            // {
            //     Assert.Fail($"Failed to navigate to {selectedDestination.Url}. Current URL: {Page.Url}. Exception: {ex.Message}");
            // }

            var boatCards = await Page.Locator("#search-result a.card").AllAsync();

    // Check if any boat cards are found
    if (boatCards.Count > 0)
    {
        // Select one randomly
        var random2 = new Random();
        var randomCard = boatCards[random2.Next(boatCards.Count)];

        // Click on the randomly selected boat card
        await randomCard.ClickAsync();
        await Page.GetByText("All Photos ").Nth(1).ClickAsync();
await Page.Locator(".boat-gallery > a:nth-child(4)").ClickAsync();
await Page.GetByLabel("Close gallery").ClickAsync();
await Page.GetByRole(AriaRole.Button, new() { Name = "close dialog" }).ClickAsync();
await Page.GetByRole(AriaRole.Button, new() { Name = "Booking Request" }).ClickAsync();

await Page.GetByRole(AriaRole.Textbox, new() { Name = "Check in" }).ClickAsync();

// Locate the first available date (today) in the "Check-in" calendar and click it
var checkInElement = Page.Locator("div.CheckinDate span.flatpickr-day[aria-current='date']").Nth(0); // Assuming "check-in-calendar" class exists for check-in
await checkInElement.ClickAsync();

// Get the day of the selected check-in date
var checkInDate = int.Parse(await checkInElement.InnerTextAsync());

// Click the "Check out" textbox to bring up the check-out calendar
await Page.GetByRole(AriaRole.Textbox, new() { Name = "Check out" }).ClickAsync();

// Use a CSS selector to locate the first available check-out date (avoiding disabled dates) in the "Check-out" calendar
var availableCheckOutDate = Page.Locator("div.CheckoutDate span.flatpickr-day:not(.flatpickr-disabled)").Nth(1); // Assuming "check-out-calendar" class exists for check-out

// Add an explicit wait to ensure the element is visible
await availableCheckOutDate.WaitForAsync(new() { State = WaitForSelectorState.Visible });

// Click the next available date (check out date)
await availableCheckOutDate.ClickAsync();


// Click on the select dropdown
await Page.Locator("select#SelectedGuest").ClickAsync();

// Select the option with value "1"
await Page.Locator("select#SelectedGuest").SelectOptionAsync(new[] { "1" });

await Page.GetByRole(AriaRole.Button, new() { Name = "Continue   →" }).ClickAsync();
await Page.Locator("#ContactVm_Name").ClickAsync();
await Page.Locator("#ContactVm_Name").FillAsync("Guest");
await Page.Locator("#ContactVm_Phone").ClickAsync();
await Page.Locator("#ContactVm_Phone").FillAsync("108641216797");
await Page.Locator("#ContactVm_Email").ClickAsync();
await Page.Locator("#ContactVm_Email").FillAsync("nomadshomoy@gmail.com");
await Page.Locator("#ContactVm_Message").ClickAsync();
await Page.Locator("#ContactVm_Message").FillAsync("auto-test");
await Page.GetByRole(AriaRole.Button, new() { Name = "Submit Request" }).ClickAsync();
        await Task.Delay(5000);
    }
    else
    {
        Console.WriteLine("No boat cards found.");
    }
        }

        [TearDown]
        public async Task TearDown()
        {
            // Close the browser after the test
            await _browser.CloseAsync();
        }
    }
}
