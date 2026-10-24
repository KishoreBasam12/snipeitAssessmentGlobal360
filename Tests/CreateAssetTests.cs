using Microsoft.Playwright;
using NUnit.Framework;
using SnipeIt.Tests.Pages;
using SnipeIt.Tests.TestData;
namespace SnipeIt.Tests.Tests;

[TestFixture]
public class CreateAssetTests : PlaywrightTestBase
{
    [Test]
    public async Task CreateAsset_CheckDetails_AndVerifyHistory()
    {
        // Get login credentials from environment variables.
        // Use default credentials if environment variables are not set.
        var username =
            Environment.GetEnvironmentVariable("SNIPEIT_USERNAME")
            ?? "admin";

        var password =
            Environment.GetEnvironmentVariable("SNIPEIT_PASSWORD")
            ?? "password";

        // Initialise the page objects.
        var loginPage = new LoginPage(Page);
        var assetsPage = new AssetsPage(Page);

        //1. Login to the Snipe-IT application.
        await loginPage.LoginAsync(username, password);

        // 2. Verify that login was successful. Assert successful login.
        await loginPage.successfulLogin();

        // 3. Create a new Macbook Pro 13 asset with Ready to Deploy status. 
        await assetsPage.CreateAssetAsync("Macbook Pro 13", "Ready to Deploy");

        // 4. Verify that the asset was created successfully.
        await assetsPage.successfulCreate("created successfully");

        // 5. Search for the newly created asset.
        await assetsPage.SearchForAssetAsync("Macbook Pro 13", "Ready to Deploy");

        // 6. Open the asset details.
        await assetsPage.openAssetsAsync();

        // 7. Open the History tab.
        await assetsPage.openHistoryTabAsync();

        // 8. Verify that the asset history contains a checkout entry.
        await assetsPage.VerifyHistoryAsync("checkout");
    }
}


