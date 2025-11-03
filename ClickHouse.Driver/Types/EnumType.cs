using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClickHouse.Driver.Formats;
using ClickHouse.Driver.Types.Grammar;

namespace ClickHouse.Driver.Types;

internal class EnumType : ParameterizedType
{
    private Dictionary<string, int> values = new Dictionary<string, int>();

    public override string Name => "Enum";

    public override Type FrameworkType => typeof(string);

    public override ParameterizedType Parse(SyntaxTreeNode node, Func<SyntaxTreeNode, ClickHouseType> parseClickHouseTypeFunc, TypeSettings settings)
    {
        var parameters = node.ChildNodes
            .Select(cn => cn.Value)
            .Select(p => p.Split('='))
            .ToDictionary(kvp => kvp[0].Trim().Trim('\''), kvp => Convert.ToInt32(kvp[1].Trim(), CultureInfo.InvariantCulture));

        string typeName = TypeConverter.ExtractTypeName(node);

        switch (typeName)
        {
            case "Enum":
            case "Enum8":
                return new Enum8Type { values = parameters };
            case "Enum16":
                return new Enum16Type { values = parameters };
            default: throw new ArgumentOutOfRangeException($"Unsupported Enum type: {node.Value}");
        }
    }

    public int Lookup(string key) => values[key];

    public string Lookup(int value) => values.SingleOrDefault(kvp => kvp.Value == value).Key ?? throw new KeyNotFoundException();

    public override string ToString() => $"{Name}({string.Join(",", values.Select(kvp => kvp.Key + "=" + kvp.Value))}";

    public override object Read(ExtendedBinaryReader reader) => throw new NotImplementedException();

    public override void Write(ExtendedBinaryWriter writer, object value) => throw new NotImplementedException();
}
