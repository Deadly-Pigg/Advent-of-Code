using System.Text;
using System.Text.RegularExpressions;
public class Day01_2024 : Days<int>
{
    private int[] left = new int[0];
    private int[] right = new int[0];
    public override int Answer(bool part2) 
    {
        if(!part2)
        {
            Parse();
            return Part1();
        }
        return Part2();
    }

    public int Part1()
    {
        int sum = 0;

        for(int i = 0; i < left.Length; i++)
            sum += Math.Abs(left[i] - right[i]);
    
        return sum;
    }

    public int Part2()
    {
        int ans = 0;
        Dictionary<int,int> rightCount = new();

        foreach(int num in right)
        {
            if(!rightCount.ContainsKey(num))
                rightCount[num] = 0;
            rightCount[num]++;
        }
        
        for(int i = 0; i < left.Length; i++)
        {
            if(rightCount.ContainsKey(left[i]))
                ans += left[i] * rightCount[left[i]];
        }

        return ans;
    }

    private void Parse()
    {
        string[] file = File.ReadAllLines("01_2024.txt"); //reading from the file content
        left = new int[file.Length];
        right = new int[file.Length]; //arrays to store numbers

        for(int i = 0; i < file.Length; i++) //parsing each line in file
        {
            var matches = Regex.Matches(file[i], @"\d+");
            left[i] = Helpers_2024.NumberParse(matches[0],i);
            right[i] = Helpers_2024.NumberParse(matches[1],i);
        }

        Array.Sort(left); //sorting arrays for part 1
        Array.Sort(right);
    }
}
