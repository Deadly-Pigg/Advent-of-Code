using System.Text;
using System.Text.RegularExpressions;
public class Day03_2024 : Days<int>
{
    string file = File.ReadAllText("03_2024.txt");
    public override int Answer(bool part2) 
    {
        if(!part2)
            return Part1(file);
        return Part2();
    }
    public int Part1(string memory)
    {
        int total = 0;
        var matches = Regex.Matches(memory, @"mul\(\d{1,3},\d{1,3}\)");
        foreach(Match m in matches)
        {
            var nums = Regex.Matches(m.Value, @"\d+");
            total += Helpers_2024.NumberParse(nums[0], 0) * Helpers_2024.NumberParse(nums[1], 0);
        }
        return total;
    }
    public int Part2()
    {
        string newMem = Regex.Replace(file, @"don't\(\).*?($|do\(\))", @"", RegexOptions.Singleline);
        return Part1(newMem);
    }
}
