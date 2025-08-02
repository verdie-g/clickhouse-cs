using System;

namespace ClickHouse.Driver.ADO;

internal static class ClickHouseEnvironment
{
    public static string Database => Environment.GetEnvironmentVariable("CLICKHOUSE_DB") ?? "default";

    public static string Username => Environment.GetEnvironmentVariable("CLICKHOUSE_USER") ?? "default";

    public static string Password => Environment.GetEnvironmentVariable("CLICKHOUSE_PASSWORD") ?? string.Empty;
}
