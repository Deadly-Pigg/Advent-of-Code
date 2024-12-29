using System.Text;
using System.Text.RegularExpressions;
public class Day01_2023 : Days<int>
{
    private string[] replaceValues = { "nine", "eight", "seven", "six", "five", "four", "three", "two", "one" };   
    private string[] toReplaceTo = { "9", "8", "7", "6", "5", "4", "3", "2", "1" }; //replacement values. Replaces words with their respective numbers.
    public override int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC1.txt"); //reading from the file that's in the bin

        return part2 == true ? Part2(file,0) : Part1(file,0);
    }
    private int Part1(string[] file, int sum) 
    {
        for(int i = 0; i < file.Length; i++) //loops through each line in the input
        { 
            Match first = Regex.Match(file[i], @"\d", 0);
            Match last = Regex.Match(file[i], @"\d", RegexOptions.RightToLeft);
            sum += int.Parse(first.Value + last.Value); //takes first and last number, and adds them to the sum.
        }
        return sum;
    }
    private int Part2(string[] file, int sum) 
    {
        for(int i = 0; i < file.Length; i++) 
            sum += int.Parse(ReplaceFirst(file[i]));

        return sum;
    }

    private string ReplaceFirst(string newline) 
    { //for part 2. converts words to numbers. This function aims to work inwards until it finds the first and last integer/word number
        int length = newline.Length;

        /*boundaries for the strings. basically, if a string has a sequence like 'o1seven...', we want the boundary to end at 1, as that's the number 
        * we're going to use in the final sum, so there's no need to convert seven.*/
        int lastBoundary = Regex.Match(newline, @"\d", RegexOptions.RightToLeft).Index;
        int boundary = Regex.Match(newline, @"\d").Index; 

        StringBuilder last = new StringBuilder(); 
        StringBuilder first = new StringBuilder();
        
        for (int i = 0; i <= boundary; i++) //incase there's any strings that are word numbers, we convert the first and last one, respectively
        { 
            //first string number
            if (i != first.Length) //once a number has been converted, it ends the loop since there's no need to do any further operations
                break;

            first.Append(newline[i]); //appending to the stringbuilder the first character

            for (int j = 0; j < 9; j++) //replaces any words with their numbers once.
            {
                first = first.Replace(replaceValues[j], toReplaceTo[j]);
            }
        }

        for (int i = length-1; i >= lastBoundary; i--) //last string number. all operations are the same, but in reverse.
        { 

            if (length-i-1 != last.Length)
                break;

            last.Insert(0,newline[i]);

            for (int j = 0; j < 9; j++) 
            {
                last = last.Replace(replaceValues[j], toReplaceTo[j]);
            }
        }
        //Console.WriteLine(first.ToString() + ",,," + last.ToString()); //this will give a good idea of what's going on here
        return first[^1] + "" + last[0];
    }
}
