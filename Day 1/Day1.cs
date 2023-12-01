using System.Text;
using System.Text.RegularExpressions;
public class Day1
{
    private string[] replaceValues = { "nine", "eight", "seven", "six", "five", "four", "three", "two", "one" };   
    private string[] toReplaceTo = { "9", "8", "7", "6", "5", "4", "3", "2", "1" };
    public int Answer(bool part2) {
        string[] file = File.ReadAllLines("AoC1.txt"); //reading from file in the bin
        
        int sum = 0;

        foreach (string line in file) { //loops through each line in the input
        string newline = line;
        if(part2)
            newline = ReplaceFirst(line); //part 2, comment out for part 1.
        newline = Regex.Replace(newline, @"\D", ""); //gets rid of all non-integer characters
        sum += 10 * (newline[0] - '0') + (newline[newline.Length - 1] - '0'); //takes first and last number, and adds them to the sum.
        }
        return sum;
    }
    //for part 2. converts words to numbers. This function aims to work inwards until it finds the first and last integer/word number
    private string ReplaceFirst(string newline) { 
        int length = newline.Length;
        int boundary = -1;
        int lastBoundary = -1;

        StringBuilder last = new StringBuilder();
        StringBuilder first = new StringBuilder();

        /*boundaries for the strings. basically, if a string has a sequence like 'o1seven...', we want the boundary to end at 1, as that's the number 
        * we're going to use in the final sum, so there's no need to convert seven in this hypothetical scenario*/
        for (int i = 0; i < length; i++) {         
            int temp = newline[i] - '0';
            if (temp >= 1 && temp <= 9) {
                if(boundary == -1)
                    boundary = i;
                lastBoundary = i;
            }
        }
        
        for (int i = 0; i <= boundary; i++) { //incase there's any strings that are word numbers, we convert the first and last one, respectively
            //first string number
            if (i != first.Length) //once a number has been converted, it ends the loop since there's no need to do any further operations
                break;

            first.Append(newline[i]); //appending to the stringbuilder the first character

            for (int j = 0; j < 9; j++) { //replaces any words with their numbers once.
                first = first.Replace(replaceValues[j], toReplaceTo[j]);
            }
        }

        for (int i = length-1; i >= lastBoundary; i--) { //last string number. all operations are the same, but in reverse.

            if (length-i-1 != last.Length)
                break;

            last.Insert(0,newline[i]);

            for (int j = 0; j < 9; j++) {
                last = last.Replace(replaceValues[j], toReplaceTo[j]);
            }
        }

        return first.ToString() + last.ToString(); //returns full string.
    }
}
