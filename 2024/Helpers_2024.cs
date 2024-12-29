
using System.Text.RegularExpressions;

public class Helpers_2024
{
    /*
    Used for ensuring that regex-matched values are integers
    */
    public static int NumberParse(Group match, int line)
    {
        if(int.TryParse(match.Value, out int number))
            return number;
        throw new Exception("File line " + line + " has an invalid value (can not be parsed to Int32). Error element:" + match.Value);
    }
}