namespace AccountManager.Data;

public static class Connection
{
    private const string ConnectionString = "Server=localhost;Port=5432;Username=test;Password=test;Database=AccountManager";

    public static string GetConnectingString()
    {
        return ConnectionString;
    }
  
}