# Snipe-IT Playwright Automation Assessment

## Overview

This project automates the asset creation and verification workflow in the Snipe-IT demo application using C#, .NET, Playwright and NUnit.

The framework follows the Page Object Model (POM) to keep page actions and test logic organised.

## Technologies Used

- C#
- .NET 10
- Microsoft Playwright
- NUnit
- Page Object Model (POM)

## Project Structure

```text
SnipeIt.Tests/
├── Pages/
│   ├── LoginPage.cs
│   ├── CreateAssetFormPage.cs
│   ├── AssetsPage.cs
│   ├── AssetDetailsPage.cs
│   └── HistoryPage.cs
├── Tests/
│   └── CreateAssetTests.cs
├── TestData/
│   └── AssetTestData.cs
├── PlaywrightTestBase.cs
├── SnipeIt.Tests.csproj
└── README.md
```

## Test Scenario

The automated test performs the following steps:

1. Opens the Snipe-IT demo application and logs in.
2. Verifies that login is successful.
3. Creates a Macbook Pro 13" asset.
4. Sets the asset status to Ready to Deploy.
5. Selects a random user for checkout.
6. Verifies the asset creation success message.
7. Searches for the newly created asset.
8. Opens the asset details page and verifies the asset tag, status and assigned user.
9. Opens the History tab and verifies the checkout entry, asset tag and user.

## Prerequisites

- .NET 10 SDK
- A supported browser installed through Playwright
- Access to the Snipe-IT demo application

Demo URL: https://demo.snipeitapp.com/login

## Configuration

The test supports environment variables for the login credentials:

- `SNIPEIT_USERNAME`
- `SNIPEIT_PASSWORD`

Set these variables in PowerShell before running the test if you do not want to use the default credentials configured in the test code.

## How to Run

Open PowerShell in the project directory and run:

```powershell
dotnet restore
dotnet build
```

Install the Playwright browser required by the project:

```powershell
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

If `pwsh` is unavailable on Windows, use the generated Playwright script with Windows PowerShell, if supported by your installed Playwright version.

Run the automated test:

```powershell
dotnet test
```

## Framework Design

- **LoginPage:** Handles login and verifies successful login.
- **CreateAssetFormPage:** Creates the asset, selects the model and status, selects a random user, and captures the asset tag.
- **AssetsPage:** Verifies the success message and searches for the created asset.
- **AssetDetailsPage:** Verifies the asset tag, status and assigned user.
- **HistoryPage:** Verifies the checkout history entry.
- **CreateAssetTests:** Executes the complete end-to-end test scenario.
- **AssetTestData:** Stores expected test values.
- **PlaywrightTestBase:** Manages browser setup and cleanup.

## Notes

- The browser runs in headed mode so the test execution can be observed.
- The test uses Playwright assertions to validate expected results.
- A random user is selected from the available user dropdown options.
