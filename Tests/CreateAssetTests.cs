
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
        var username =
            Environment.GetEnvironmentVariable("SNIPEIT_USERNAME")
            ?? "admin";

        var password =
            Environment.GetEnvironmentVariable("SNIPEIT_PASSWORD")
            ?? "password";

        var assetTag = AssetTestData.CreateUniqueAssetTag();

        var loginPage = new LoginPage(Page);
        var assetsPage = new AssetsPage(Page);
       // var detailsPage = new AssetDetailsPage(Page);

        // 1. Login
        await loginPage.LoginAsync(username, password);
        await loginPage.successfulLogin();

        // TODO: Assert successful login.
        //  await assetsPage.CreateAssetAsync("Macbook Pro 13","Ready to Deploy");
        // // 2. Create the asset and check it out.
        // // Keep the randomly selected user's actual name
        // // so the same value can be verified below.
        // await assetsPage.SearchForAssetAsync("Macbook Pro 13","Ready to Deploy");
        // // await assetsPage.OpenAssetsAsync();
        // await assetsPage.VerifyHistoryAsync();
    }
}


