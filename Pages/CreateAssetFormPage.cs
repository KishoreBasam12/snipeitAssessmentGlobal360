using Microsoft.Playwright;
namespace SnipeIt.Tests.Pages;
public class CreateAssetFormPage
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
    // Asset details
    private string assetTag = "";
    private string user = "";
    public string AssetTag => assetTag;
    public string SelectedUser => user;

    public CreateAssetFormPage(IPage page)
    {
        _page = page;
    }

    public async Task CreateAssetAsync(string model, string status)
    {
        await CreateNewLink.ClickAsync();
        await AssetMenu.ClickAsync();
        // Select company
        await CompanyDropdown.ClickAsync();
        await CompanyOptions.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await CompanyOptions.First.WaitForAsync(new() { State = WaitForSelectorState.Attached });
        var companyOption = CompanyOptions.Filter(new() { HasTextRegex = new System.Text.RegularExpressions.Regex(@"\S") }).First;
        await companyOption.ClickAsync();
        // Get generated asset tag
        assetTag = await AssetTagInput.InputValueAsync();
        // Select model
        await ModelDropdown.ClickAsync();
        await ModelSearch.FillAsync(model);
        await ModelOptions.GetByText(model, new() { Exact = false }).ClickAsync();

        // Select status
        await StatusDropdown.ClickAsync();
        await StatusSearch.FillAsync(status);
        await StatusOptions.GetByText(status, new() { Exact = false }).ClickAsync();
        // Select user
        await UserDropdown.ClickAsync();

        var userOptions = _page.Locator("ul#select2-assigned_user_select-results li").Filter(new() { Has = _page.Locator(".clearfix") });

        await userOptions.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        int count = await userOptions.CountAsync();
        int randomIndex = Random.Shared.Next(count);

        var userOption = userOptions.Nth(randomIndex);
        user = (await userOption.InnerTextAsync()).Trim();

        await userOption.ClickAsync();
        // Save asset
        await SaveButton.ClickAsync();
        
    }
}
