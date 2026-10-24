
namespace SnipeIt.Tests.TestData;

public static class AssetTestData
{
    public static string CreateUniqueAssetTag()
    {
        return $"PW-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..30];
    }

    public const string AssetModel = "Macbook Pro 13\"";
    public const string Status = "Ready to Deploy";
}
