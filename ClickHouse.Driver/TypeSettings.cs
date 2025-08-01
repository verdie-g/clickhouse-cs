using NodaTime;

namespace ClickHouse.Driver;

internal record struct TypeSettings(bool useBigDecimal, string timezone)
{
    public static string DefaultTimezone = DateTimeZoneProviders.Tzdb.GetSystemDefault().Id;

    public static TypeSettings Default => new TypeSettings(true, DefaultTimezone);
}
