using System;
using ClickHouse.Driver.Types.Grammar;

namespace ClickHouse.Driver.Types;

internal abstract class ParameterizedType : ClickHouseType
{
    public abstract string Name { get; }

    public abstract ParameterizedType Parse(SyntaxTreeNode typeName, Func<SyntaxTreeNode, ClickHouseType> parseClickHouseTypeFunc, TypeSettings settings);
}
