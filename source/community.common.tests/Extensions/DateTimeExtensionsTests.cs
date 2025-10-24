namespace community.common.tests.Extensions;

[ExcludeFromCodeCoverage]
public class DateTimeExtensionsTests
{
    private static readonly long Jan2025 = 1735689600;

    [Fact]
    [Trait("Category", "Unit")]
    public void ToUnixTime_AndBack_ShouldSucceed()
    {
        var originalDate = new DateTime(1980, 1, 25, 11, 32, 56, DateTimeKind.Utc);
        var epoch = originalDate.ToUnixTime();
        Assert.True(Jan2025 > epoch);
        var epochToDateTime = epoch.FromUnixTime();
        Assert.Equal(originalDate, epochToDateTime);
    }
}