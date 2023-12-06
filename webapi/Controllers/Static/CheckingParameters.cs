namespace webapi.Controllers.Static;

public class CheckingParameters
{
    public static bool AreParametersInvalid(params object?[] parameters)
    {
        return !parameters.All(parameter => parameter is not null);
    }

    public static bool AreIntParametersDefault(params int[] parameters)
    {
        return parameters.Any(parameter => parameter == default);
    }

    public static bool CheckCredentials()
    {
        throw new NotImplementedException();
    }

    public static bool CheckGameInfo()
    {
        throw new NotImplementedException();
    }
}