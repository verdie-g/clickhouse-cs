namespace ClickHouse.Driver.Types;

internal abstract class IntegerType : ClickHouseType
{
    public virtual bool Signed => true;
}
