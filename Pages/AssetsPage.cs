using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
namespace SnipeIt.Tests.Pages;
public class AssetsPage
{
    private readonly IPage _page;

    private ILocator SuccessMessage =>
        _page.Locator(".alert.alert-success");

    private ILocator SearchBox =>
        _page.GetByRole(AriaRole.Searchbox, new() { Name = "Search" });

    private ILocator AssetRows =>
        _page.Locator("#assetsListingTable tbody tr");

    public AssetsPage(IPage page)
    {
        _page = page;
    }
    private string assetTag = "";
    private string user = "";
    private ILocator? assetRow;

    public string AssetTag => assetTag;
    public string SelectedUser => user;

    public async Task successfulCreate(string success)
    {
        await Expect(SuccessMessage).ToContainTextAsync("Asset with tag");
        await Expect(SuccessMessage).ToContainTextAsync(success);
    }

    public async Task SearchForAssetAsync(string assetTag, string model, string status, string user)
    {
        await SearchBox.FillAsync(assetTag);
        await SearchBox.PressAsync("Enter");

        assetRow = AssetRows.Filter(new() { HasText = assetTag });

        await Expect(assetRow).ToHaveCountAsync(1);
        await Expect(assetRow).ToContainTextAsync(assetTag);
        await Expect(assetRow).ToContainTextAsync(model);
        await Expect(assetRow).ToContainTextAsync(status);

        var expectedUser = System.Text.RegularExpressions.Regex
            .Replace(user, @"\s*-\s*#\d+", "");

        expectedUser = System.Text.RegularExpressions.Regex
            .Replace(expectedUser, @"\s+", " ").Trim();

        await Expect(assetRow).ToContainTextAsync(expectedUser);
    }

    public async Task OpenAssetAsync(string assetTag)
    {
        await assetRow!.GetByRole(
            AriaRole.Link, new() { Name = assetTag }).ClickAsync();
    }
}
