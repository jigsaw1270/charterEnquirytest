
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


            await Page.Locator("h2.title.mark-accent.mt-6.mb-3:has-text('Our Yachting Destinations in the Balearics')")
                .WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
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


            await Page.Locator(selectedDestination.Selector).ClickAsync();


            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 10000 });

            await Expect(Page).ToHaveURLAsync(new Regex(selectedDestination.Url), new() { Timeout = 5000 });


            var boatCards = await Page.Locator("#search-result a.card").AllAsync();


            if (boatCards.Count > 0)
            {

                var random2 = new Random();
                var randomCard = boatCards[random2.Next(boatCards.Count)];
                await randomCard.ClickAsync();


                // code to  check  gallery !!if needed!!
                // await Page.GetByText("All Photos ").Nth(1).ClickAsync();
                // await Page.Locator(".boat-gallery > a:nth-child(4)").ClickAsync();
                // await Page.GetByLabel("Close gallery").ClickAsync();
                // await Page.GetByRole(AriaRole.Button, new() { Name = "close dialog" }).ClickAsync();


                await Page.GetByRole(AriaRole.Button, new() { Name = "Booking Request" }).ClickAsync();


                var checkInDateInput = Page.Locator("#CheckinDate");

                if (await checkInDateInput.CountAsync() == 0)
                {
                    Assert.Fail("Check-in date input not found.");
                }
                await checkInDateInput.ClickAsync();


                await Page.WaitForSelectorAsync(".flatpickr-calendar.open");

                // Select the first available check-in day
                var firstAvailableCheckInDay = Page.Locator(".flatpickr-day:not(.flatpickr-disabled):not(.prevMonthDay):not(.nextMonthDay)").Nth(0);
                if (await firstAvailableCheckInDay.CountAsync() == 0)
                {
                    Assert.Fail("No available check-in date found.");
                }
                await firstAvailableCheckInDay.ClickAsync();

                // Wait for the check-in calendar to close
                await Page.WaitForSelectorAsync(".flatpickr-calendar.open", new PageWaitForSelectorOptions { State = WaitForSelectorState.Hidden });


                var checkOutDateInput = Page.Locator("#CheckoutDate");


                // Explicit wait for the element to be visible
                await checkOutDateInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

                await checkOutDateInput.ClickAsync();
                await Page.WaitForSelectorAsync(".flatpickr-calendar.open", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });


                var currentDayElement = Page.Locator("span.flatpickr-day.today[aria-current='date']");

                // Find the next available day by iterating over following siblings !! needed for  the  boats  which  has longer booking requirements 3/5/7 days !!
                bool nextDayFound = false;
                int siblingIndex = 1;

                while (!nextDayFound)
                {

                    var nextDayElement = currentDayElement.Locator($"xpath=following-sibling::span[{siblingIndex}]").Nth(1);



                    if (await nextDayElement.CountAsync() == 0)
                    {
                        Console.WriteLine("No more available days");
                        break;
                    }


                    var classAttribute = await nextDayElement.GetAttributeAsync("class");


                    bool isDisabled = classAttribute != null && classAttribute.Contains("flatpickr-disabled");

                    if (!isDisabled)
                    {

                        await nextDayElement.ClickAsync();
                        nextDayFound = true;
                        Console.WriteLine("Next available day clicked");
                    }
                    else
                    {
                        // Increment the index to move to the next sibling
                        siblingIndex++;
                    }
                }


                // guest-dropdown
                await Page.Locator("select#SelectedGuest").ClickAsync();

                await Page.Locator("select#SelectedGuest").SelectOptionAsync(new[] { "1" });

                await Page.GetByRole(AriaRole.Button, new() { Name = "Continue   →" }).ClickAsync();
                //form-page
                await Page.Locator("#ContactVm_Name").ClickAsync();
                await Page.Locator("#ContactVm_Name").FillAsync("Guest");
                await Page.Locator("#ContactVm_Phone").ClickAsync();
                await Page.Locator("#ContactVm_Phone").FillAsync("108641216797");
                await Page.Locator("#ContactVm_Email").ClickAsync();
                await Page.Locator("#ContactVm_Email").FillAsync("nomadshomoy@gmail.com");
                await Page.Locator("#ContactVm_Message").ClickAsync();
                await Page.Locator("#ContactVm_Message").FillAsync("auto-test");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Submit Request" }).ClickAsync();
                await Task.Delay(2000);
                await Page.WaitForSelectorAsync("div.confirm-alert");

                // passes the  test if matches  the  confirm  alert
                var confirmAlert = Page.Locator("div.confirm-alert");
                await Expect(confirmAlert).ToBeVisibleAsync();
                await Expect(confirmAlert).ToHaveTextAsync("Thank you for your request, one of our colleagues will contact you shortly.");

            }
            else
            {
                Console.WriteLine("No boat cards found.");
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }
    }
}
