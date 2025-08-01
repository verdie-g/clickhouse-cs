using System.IO;

namespace ClickHouse.Driver.Copy.Serializer;

internal interface IBatchSerializer
{
    void Serialize(Batch batch, Stream stream);
}
