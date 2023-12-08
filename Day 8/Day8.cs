using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
public class Day8 : Days<long>
{
    Dictionary<string, string[]> pathChooser = new(); //for the paths. key = current position, values = next paths
    List<string> starts = new(); //for all the positions ending in 'A'
    public override long Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC8.txt"); 
        string instructions = file[0];

        if(pathChooser.Count == 0) //incase the pathChooser dictionary is already filled.
        { //This just parses the file.
            for(int i = 2; i < file.Length; i++)
            {
                MatchCollection regex = Regex.Matches(file[i], @"\w+");
                pathChooser.Add(regex[0].Value, new string[] {regex[1].Value, regex[2].Value});
                if(regex[0].Value[2] == 'A')
                    starts.Add(regex[0].Value);
            }
        }

        if(part2)
        {
            long sum = FollowPaths(instructions, 0, starts[0]);

            for(int i = 1; i < starts.Count(); i++) //for finding the multiple of minimum steps to go from 'A' to 'Z' in each case
            {
                long temp = FollowPaths(instructions, 0, starts[i]); 
                sum /= GCD(sum, temp);
                sum *= temp;
            }

            pathChooser.Clear(); //clearing up memory so my PC doesn't suffer.
            starts.Clear();

            return sum;

        }
        return FollowPaths(instructions, 0, "AAA"); //part 1

    }
    private long GCD(long num1, long num2) //Greatest Common divisor. Since all 'A' paths have only 1 'Z' end (when following the instructions), finding the 
    {                                      //minimum steps required just needs to find the greatest multiple of all paths.
        long Remainder;
        if(num2 > num1) 
        {
            long temp = num2;
            num2 = num1;
            num1 = temp;
        }

        while(num2 * 10 < num1) //simply speeds up the GCD finder. (this is incase num1 is something like 1 and num2 is 2,147,483,647, which would slow it down)
            num2 *= 10;
 
        while (num2 != 0)
        {
            Remainder = num1 % num2;
            num1 = num2;
            num2 = Remainder;
        }
        return num1;
    }

    private long FollowPaths(string instruct, int i, string curr) //Finds the closest 'Z' end for each 'A' path.
    {                                                             //This works for part 1 aswell since the closest end to 'AAA' is 'ZZZ' when following the instructions.
        long count = 0;
        while(curr[2] != 'Z')
        {
            if(instruct[i] == 'R')
                curr = pathChooser[curr][1];
            else
                curr = pathChooser[curr][0];
            i++;
            i %= instruct.Length;
            count++;
        }
        return count;
    }
}
