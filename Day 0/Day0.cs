using System.Text.RegularExpressions;
using System.Text;

public class Day0 : Days<string>
{
    public override string Answer(bool part2) {
        StringBuilder initialiser = new StringBuilder(Regex.Replace(@"333333", @"\d", ""));
        if(!part2)
            return "setting up the benchmarking";
        return "you can ignore this";
    }
}