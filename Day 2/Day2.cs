using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
public class Day2
{
    public int Answer(bool part2) {
        string[] file = File.ReadAllLines("AoC2.txt"); //reading from file in the bin
        int sum = 0;

        foreach(string line in file) {
            string newline = Regex.Replace(line, @"^[^\d:]+(\d{1,}): ", "");
            int blue = Max(Regex.Matches(newline, @"\d+(?=.b)"));
            int red = Max(Regex.Matches(newline, @"\d+(?=.r)"));
            int green = Max(Regex.Matches(newline, @"\d+(?=.g)"));

            if(part2)
                sum += blue*red*green;
            else if(red < 13 && green < 14 && blue < 15)
                sum += ToInteger(Regex.Replace(line, @"^[^\d:]+(\d{1,}): .+", @"$+"));
        }
        return sum;
    }
    
    private int Max(MatchCollection reg) {
        int max = 0;
        foreach(Match m in reg) {
            int contender = ToInteger(m.Value);
            if(contender > max)
                max = contender;
        }
        return max;
    }
    private int ToInteger(string input) {
        int num = 0;
        foreach(char c in input) {
            num *= 10;
            num += c - '0';
        }
        return num;
    }

}
