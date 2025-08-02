using ClickHouse.Driver.Formats;
using ClickHouse.Driver.Types;

namespace ClickHouse.Driver.Copy.Serializer;

internal interface IRowSerializer
{
    void Serialize(object[] row, ClickHouseType[] types, ExtendedBinaryWriter writer);
}
