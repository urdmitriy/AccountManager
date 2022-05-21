using System.Text;

namespace AccountManager.Data;

public static class Service
{
    public static string GeneratePassword(int lenghtPassword)
    {
        char[] template =
        {
            '1','2','3','4','5','6','7','8','9','q','w','e','r','t','y','u','p','a','s','d','f','g','h','k','z','x','c','v','b','n','m',
            'Q','W','E','R','T','Y','U','P','A','S','D','F','G','H','K','Z','X','C','V','B','N','M'
        };
        var result = new StringBuilder();
        var rnd = new Random();
        for (int i = 0; i < lenghtPassword; i++)
        {
            result.Append(template[rnd.Next(0, template.Length)]);
        }

        return result.ToString();
    }
}