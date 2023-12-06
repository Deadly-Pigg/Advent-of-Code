using System.Text.RegularExpressions; 
public class Day2 : Days<int>
{
    public override int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC2.txt"); //reading from file in the bin
        int sum = 0; //sum

        foreach(string line in file) //looping though each line. 
        { //Note: Yes, this will be repetitive. I may not comment this for every day. Maybe just say a random joke or something from now on

        //this section parses the string
            string newline = Regex.Replace(line, @"^[^\d:]+(\d{1,}): ", ""); //removes the 'Game #: ' part from the string
            int blue = Max(Regex.Matches(newline, @"\d+(?=.b)")); //returns all numbers that are followed by a 'b '
            int red = Max(Regex.Matches(newline, @"\d+(?=.r)"));  //this is since we want the cubes for each colour
            int green = Max(Regex.Matches(newline, @"\d+(?=.g)"));//Also gets the max out of said numbers

            if(part2)  //'power' of the cubes as requested in part 2
                sum += blue*red*green; 
            else if(red < 13 && green < 14 && blue < 15) //Part 1. If the game meets the criteria, it adds the game number to the sum
                sum += int.Parse(Regex.Match(line, @"\d+").Value); 
        }
        return sum;
    }
    
    private int Max(MatchCollection reg) 
    { 
        int max = 0;
        foreach(Match m in reg) //Loops through each matched item from the regular expression, trying to find the max number.
        { 
            int contender = int.Parse(m.Value);
            if(contender > max) 
                max = contender;
        }
        return max;
    }
}
