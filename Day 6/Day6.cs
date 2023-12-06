using System.Text.RegularExpressions;
public class Day6 : Days<double>
{
    public override double Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC6.txt"); 
        if(part2) 
        {
            file[0] = Regex.Replace(file[0], " ", "");
            file[1] = Regex.Replace(file[1], " ", "");
        }
        MatchCollection regexTime = Regex.Matches(file[0],@"(\d+)"); 
        MatchCollection regexRecord = Regex.Matches(file[1],@"(\d+)"); 
        double sum = 1;

        for(int i = 0; i < regexTime.Count; i++)
        {
            double b = double.Parse(regexTime[i].Value);
            double c = double.Parse(regexRecord[i].Value);
            double sqrtVal = Math.Sqrt(b*b - 4*c) / 2;
            double lower = Math.Floor(b*0.5 - sqrtVal);
            double higher = Math.Ceiling(b*0.5 + sqrtVal);
            sum *= higher - lower - 1;
        }
        return sum;
    }
    
}
