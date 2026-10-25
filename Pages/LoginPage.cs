using Microsoft.Playwright;
namespace SnipeIt.Tests.Pages;
public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    private ILocator UsernameInput => _page.Locator("input[name='username'], input[name='email'], input[id='username']").First;
    private ILocator PasswordInput => _page.Locator("#password-field");

    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login", Exact = false });
    private ILocator DashboardHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" });
    public async Task LoginAsync(string username, string password)
    {
        await _page.GotoAsync(
            "https://demo.snipeitapp.com/login");

        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
    public async Task successfulLogin()
    {
        await Assertions.Expect(DashboardHeading).ToBeVisibleAsync();
    }
}
