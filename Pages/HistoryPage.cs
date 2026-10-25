using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
namespace SnipeIt.Tests.Pages;

public class HistoryPage
{
          private readonly IPage _page;

          // Page locators
          private ILocator HistoryTab =>
              _page.GetByRole(AriaRole.Link, new() { Name = "History" });

          private ILocator HistoryRows =>
              _page.Locator("#assetHistoryListingTable tbody tr");

          public HistoryPage(IPage page)
          {
                    _page = page;
          }

          public async Task OpenHistoryTabAsync()
          {
                    await HistoryTab.ClickAsync();
          }

          public async Task VerifyHistoryAsync(
              string checkout, string assetTag, string user)
          {
                    var historyRow = HistoryRows.Nth(1);
                    var expectedUserName = user.Split(" (")[0].Trim();

                    await Expect(historyRow.Locator("td:nth-child(4)")).ToContainTextAsync(checkout);

                    await Expect(historyRow.Locator("td:nth-child(5)")).ToContainTextAsync(assetTag);

                    await Expect(historyRow.Locator("td:nth-child(6)")).ToContainTextAsync(expectedUserName);
          }
}
