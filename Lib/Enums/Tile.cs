using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lib.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Tile
{
    Cross,
    Circle,
    Empty
}