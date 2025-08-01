using System;
using System.Buffers;
using ClickHouse.Driver.Types;

namespace ClickHouse.Driver.Copy;

// Convenience argument collection
internal struct Batch : IDisposable
{
    public object[][] Rows;
    public int Size;
    public string Query;
    public ClickHouseType[] Types;

    public void Dispose()
    {
        if (Rows != null)
        {
            ArrayPool<object[]>.Shared.Return(Rows, true);
            Rows = null;
        }
    }
}
