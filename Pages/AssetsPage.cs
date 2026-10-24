
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace SnipeIt.Tests.Pages;

public class AssetsPage
{
    private readonly IPage _page;

    // Form locators
    private ILocator CreateNewLink =>
        _page.GetByText("Create New", new() { Exact = true });

    private ILocator AssetMenu =>
        _page.GetByRole(AriaRole.Navigation)
             .GetByText("Asset", new() { Exact = true });

    private ILocator CompanyDropdown =>
        _page.Locator("#select2-company_id_select-container");

    private ILocator CompanyOptions =>
        _page.Locator("ul#select2-company_id_select-results li");

    private ILocator AssetTagInput =>
        _page.Locator("#asset_tag");

    private ILocator ModelDropdown =>
        _page.Locator("#select2-model_select_id-container");

    private ILocator ModelSearch =>
        _page.Locator("input[aria-controls='select2-model_select_id-results']");

    private ILocator ModelOptions =>
        _page.Locator("ul#select2-model_select_id-results li");

    private ILocator StatusDropdown =>
        _page.Locator("#select2-status_select_id-container");

    private ILocator StatusSearch =>
        _page.Locator("input[aria-controls='select2-status_select_id-results']");

    private ILocator StatusOptions =>
        _page.Locator("ul#select2-status_select_id-results li");

    private ILocator UserDropdown =>
        _page.Locator("#select2-assigned_user_select-container");

    private ILocator UserOptions =>
        _page.Locator("ul#select2-assigned_user_select-results li");

    private ILocator SaveButton =>
        _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).First;

    private ILocator SuccessMessage =>
        _page.Locator(".alert.alert-success");

    private ILocator SearchBox =>
        _page.GetByRole(AriaRole.Searchbox, new() { Name = "Search" });

    private ILocator AssetRows =>
        _page.Locator("#assetsListingTable tbody tr");

    private ILocator HistoryTab =>
        _page.GetByRole(AriaRole.Link, new() { Name = "History" });

    private ILocator HistoryRows =>
        _page.Locator("#assetHistoryListingTable tbody tr");

    // Asset details
    private string assetTag = "";
    private string user = "";
    private ILocator? assetRow;

    public string AssetTag => assetTag;
    public string SelectedUser => user;

    public AssetsPage(IPage page)
    {
        _page = page;
    }

    public async Task CreateAssetAsync(string model, string status)
    {
        await CreateNewLink.ClickAsync();
        await AssetMenu.ClickAsync();

        // Select company
        await CompanyDropdown.ClickAsync();

        await CompanyOptions.First.WaitForAsync(
            new() { State = WaitForSelectorState.Visible });

        await CompanyOptions.First.WaitForAsync(
            new() { State = WaitForSelectorState.Attached });

        var companyOption = CompanyOptions
            .Filter(new() { HasTextRegex = new System.Text.RegularExpressions.Regex(@"\S") })
            .First;

        await companyOption.ClickAsync();

        // Get generated asset tag
        assetTag = await AssetTagInput.InputValueAsync();

        // Select model
        await ModelDropdown.ClickAsync();
        await ModelSearch.FillAsync(model);

        await ModelOptions
            .GetByText(model, new() { Exact = false })
            .ClickAsync();

        // Select status
        await StatusDropdown.ClickAsync();
        await StatusSearch.FillAsync(status);

        await StatusOptions
            .GetByText(status, new() { Exact = false })
            .ClickAsync();

        // Select user
        await UserDropdown.ClickAsync();

        // Wait until the user list finishes loading
        await _page.Locator(
            "ul#select2-assigned_user_select-results li .clearfix"
        ).First.WaitForAsync(
            new() { State = WaitForSelectorState.Visible });

        // Get the first actual user option
        var userOption = _page.Locator(
            "ul#select2-assigned_user_select-results li"
        ).Filter(new()
        {
            Has = _page.Locator(".clearfix")
        }).First;

        user = (await userOption.InnerTextAsync()).Trim();

        await userOption.ClickAsync();
        // Save asset
        await SaveButton.ClickAsync();

        await Expect(SuccessMessage)
            .ToContainTextAsync(
                $"Asset with tag {assetTag} was created successfully.");
    }

    public async Task SearchForAssetAsync(string model, string status)
    {
        await SearchBox.FillAsync(assetTag);
        await SearchBox.PressAsync("Enter");

        assetRow = AssetRows.Filter(new() { HasText = assetTag });

        await Expect(assetRow).ToHaveCountAsync(1);
        await Expect(assetRow).ToContainTextAsync(assetTag);
        await Expect(assetRow).ToContainTextAsync(model);
        await Expect(assetRow).ToContainTextAsync(status);

        var expectedUser = System.Text.RegularExpressions.Regex
            .Replace(user, @"\s*-\s*#\d+", "")
            .Replace("      ", " ")
            .Trim();

        await Expect(assetRow).ToContainTextAsync(expectedUser);
    }

    public async Task VerifyHistoryAsync()
    {
        await assetRow!.GetByRole(
            AriaRole.Link, new() { Name = assetTag }).ClickAsync();

        await HistoryTab.ClickAsync();

        var historyRow = HistoryRows.Nth(1);

        await Expect(historyRow.Locator("td:nth-child(4)"))
            .ToContainTextAsync("checkout");

        await Expect(historyRow.Locator("td:nth-child(5)"))
            .ToContainTextAsync(assetTag);

        var expectedUserName = user.Split(" (")[0].Trim();

        await Expect(historyRow.Locator("td:nth-child(6)"))
            .ToContainTextAsync(expectedUserName);
    }
}
