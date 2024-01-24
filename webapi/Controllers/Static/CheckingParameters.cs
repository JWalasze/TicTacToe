namespace webapi.Controllers.Static;

public static class CheckingParameters
{
    public static bool AreParametersInvalid(params object?[] parameters)
    {
        return !parameters.All(parameter => parameter is not null);
    }

    public static bool AreIntParametersDefault(params int[] parameters)
    {
        return parameters.Any(parameter => parameter == default);
    }
}