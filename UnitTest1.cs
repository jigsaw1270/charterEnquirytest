using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ExampleTest : PageTest
{
    [Test]

    public async  Task CharterEmquiry()
    {


await Page.GotoAsync("https://web-dev.bluebnc.com/en-us"); // loads page


          // clicking and assuring tittle on balearic  islands
            await Page.GetByRole(AriaRole.Link, new() { Name = "Balearic Islands Spain" }).ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Luxury Yacht Charter Balearic Islands, Spain - Day & Term Charters - Exclusive Yacht Rentals | Bluebnc");
// selects salmans  test boat
             await Page.GetByRole(AriaRole.Link, new() { Name = "Salman's Test Boat Balearic Islands, Spain" }).ClickAsync();
             await Expect(Page).ToHaveURLAsync("https://web-dev.bluebnc.com/en-us/boat/2771");  //expects  the url

              var charterEnquiryButton = Page.GetByRole(AriaRole.Button, new() { Name = "Charter Enquiry" }); //clicks  on  charter enquiry button

                          await charterEnquiryButton.ClickAsync();

                          //waits  for  checkindate and checkout date 
            
                          await Page.WaitForSelectorAsync("#CheckinDate");
                         await Page.WaitForSelectorAsync("#CheckoutDate");

                          await Page.ClickAsync("#CheckinDate");

                
                 
        var dayContainer = Page.Locator("div.dayContainer");

        var availableDates = await dayContainer.Locator("span:not(.flatpickr-disabled)").AllAsync(); //selects the first  available  test

        await availableDates.First().ClickAsync();
     

                                    await Task.Delay(1000); 

                          await Page.ClickAsync("#CheckoutDate");

                        var dayContainer2= Page.Locator("div.dayContainer").Nth(1); //seletcts the 2nd  daycontainer

        var availableDates2 = await dayContainer2.Locator("span:not(.flatpickr-disabled)").AllAsync();

        await availableDates2.First().ClickAsync();





       



                        await Page.SelectOptionAsync("#SelectedGuest", new[] { "1" });

                              await Page.ClickAsync("#btnContinue");
                           
                  // filling in boxes
                                  await Page.FillAsync("#ContactVm_Name", "John Doe");
                                  await Page.FillAsync("#ContactVm_Phone", "012847838810");
                                  await Page.FillAsync("#ContactVm_Email", "nomadshomoy@gmail.com");
                                  await Page.FillAsync("#ContactVm_Message", "Enquiry done");
                                

                                    await Page.ClickAsync("#btnBookNow");
                                
                    //ensures the  enquiry is done  !!
                                 var successMessageLocator = Page.Locator(".confirm-alert");
            await Expect(successMessageLocator).ToHaveTextAsync("Thank you for your enquiry, one of our colleagues will contact you shortly.");


                                    

//run settings  are  set  for  chomium -  dotnet  test --settings:demo.runsettings

                                                                      




            


    }
}