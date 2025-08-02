using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ClickHouse.Driver.ADO;
using ClickHouse.Driver.ADO.Parameters;

namespace ClickHouse.Driver;

public interface IClickHouseCommand : IDbCommand
{
    new ClickHouseDbParameter CreateParameter();

    Task<ClickHouseRawResult> ExecuteRawResultAsync(CancellationToken cancellationToken);

    IDictionary<string, object> CustomSettings { get; }
}
