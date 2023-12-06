using System.Text.RegularExpressions;
public class Day6 : Days<double>
{
    public override double Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC6.txt"); 
        if(part2) //simply just gets rid of the spaces between the numbers
        {
            file[0] = Regex.Replace(file[0], " ", ""); 
            file[1] = Regex.Replace(file[1], " ", "");
        }
        MatchCollection regexTime = Regex.Matches(file[0],@"(\d+)"); //converts the lines to numbers
        MatchCollection regexDist = Regex.Matches(file[1],@"(\d+)"); 
        double sum = 1;

        for(int i = 0; i < regexTime.Count; i++) //quadratic formula to find the first and last button-time that beats the record
        {
            double b = double.Parse(regexTime[i].Value);  // x = b ± √(b² - 4ac)
            double c = double.Parse(regexDist[i].Value);  // -------------------   WHERE
            double sqrtVal = Math.Sqrt(b*b - 4*c) / 2;    //        2a 
            double lower = Math.Floor(b*0.5 - sqrtVal);   //
            double higher = Math.Ceiling(b*0.5 + sqrtVal);// a = 1, b = time, c = distance
            sum *= higher - lower - 1; 
        }
        return sum;
    }
}
