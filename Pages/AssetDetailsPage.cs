using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
namespace SnipeIt.Tests.Pages;
public class AssetDetailsPage
{
          private readonly IPage _page;

          // Page locators
          private ILocator AssetTag =>
              _page.Locator(".js-copy-asset_tag");

          private ILocator AssetStatus =>
              _page.Locator(".col-md-12.well").First;

          private ILocator AssignedUser =>
              _page.Locator(".col-md-12.well a");
          private ILocator HistoryTab =>
          _page.GetByRole(AriaRole.Link, new() { Name = "History" });

          public AssetDetailsPage(IPage page)
          {
                    _page = page;
          }
          
          public async Task VerifyAsssetDetails(string assetTag, string readytodeploy, string user)
          {
                    await Expect(AssetTag).ToContainTextAsync(assetTag);
                    await Expect(AssetStatus).ToContainTextAsync(readytodeploy);
                    await Expect(AssignedUser)
                        .ToContainTextAsync(user.Split(" (")[0].Trim());
          }
          public async Task openHistoryTabAsync()
          {
                    await HistoryTab.ClickAsync();
          }
}