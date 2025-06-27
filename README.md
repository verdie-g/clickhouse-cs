# ClickHouse.Client

Official ADO.NET client for ClickHouse DB.

## Advantages

Compared to other existing .NET clients, `ClickHouse.Client` has following advantages 
* Does not have to buffer response, reducing memory usage
* Offers wider support for ClickHouse-specific types
* Is more compliant to ADO.NET standards (e.g. does not require calling 'NextResult' on `SELECT` queries)
* Works with ORM like Dapper, Linq2DB, Entity Framework Core etc.

## Acknowledgements
Originally created by [Oleg V. Kozlyuk](https://github.com/DarkWanderer)
