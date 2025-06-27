# ClickHouse.Client

Official ADO.NET client for ClickHouse DB.

## Key features

* High-throughput
* Fully supports ClickHouse-specific types:
  * Composite types: `Array`, `Tuple`, `Nullable`, `Nested`, `Map`, including combinations
  * Specialized types: `IPv4`, `IPv6`, `UUID`, `DateTime64`, `LowCardinality`, `Enum` etc.
  * Large arithmetic types: `(U)Int128`, `(U)Int256`, `Decimal128`, `Decimal256`
* Correctly handles `DateTime`, including time zones
* Supports [bulk insertion](https://github.com/DarkWanderer/ClickHouse.Client/wiki/Bulk-insertion)
* Uses compressed binary protocol over HTTP(S)
* Available for .NET Core/Framework/Standard

## Advantages

Compared to other existing .NET clients, `ClickHouse.Client` has following advantages 
* Does not have to buffer response, reducing memory usage
* Offers wider support for ClickHouse-specific types
* Is more compliant to ADO.NET standards (e.g. does not require calling 'NextResult' on `SELECT` queries)
* Works with ORM like Dapper, Linq2DB, Entity Framework Core etc.

## Acknowledgements
Originally created by [Oleg V. Kozlyuk](https://github.com/DarkWanderer)
