using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lib.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum GameStatus
{
    WonByCircle,
    WonByCross,
    Draw,
    Unfinished,
    StillInGame,
    BeingPrepared
}