using System.Text.RegularExpressions;
public class Day9 : Days<long>
{
    List<List<int>> allNums = new();
    public override long Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC9.txt"); 

        if(allNums.Count == 0)
        {
            foreach(string line in file) //parsing the input
            {
                string[] str = line.Split(" ");
                List<int> list = AddToList(str);
                allNums.Add(list);
            }
        }

        long sum = 0;

        if(!part2)
        {
            for(int i = 0; i < allNums.Count; i++) 
                sum += allNums[i][^1] + Part1(allNums[i]);
        }
        else
        {
            for(int i = 0; i < allNums.Count; i++) 
                sum += allNums[i][0] - Part2(allNums[i]);

            allNums.Clear(); //for neatness.
        }

        return sum;
    }

    private long Part1(List<int> list) //finds the next element after the last one
    {
        List<int> newList = new List<int>();
        bool isZeroes = true;
        for(int i = 0; i < list.Count - 1; i++) //gets the differences between each element and stores them in a new list
        { //we do not need to worry about an empty list since the loop would terminate before then.
            newList.Add(list[i+1] - list[i]);
            if(list[i] != 0)
                isZeroes = false;
        }
        if(isZeroes) //This acts as the base case. If the list only contains 0, we do not need to perform any operations
            return 0;
            
        return newList[^1] + Part1(newList); //I don't really know how this works, so I'll explain it for my own sake since recursion is my weakpoint
        //basically, newList[^1] finds the last element of said list. This is useful since we need to find the next number in the sequence.
        //This gets repeated until the base case, then it cascades back up this return chain. 
        // return newList[^1] + 0, last newList[^1] gets returned. I'll name it 'val1'
        // return newList[^1] + val1, returns the sum, I'll name it val2
        // return newList[^1] + val2. So on an so forth

        // After all that, we have the next difference between the initial last number in the file and the predicted next number in the file, and so that gets returned.
    }

    private long Part2(List<int> list) //finds the element before the first
    {
        List<int> newList = new List<int>();
        bool isZeroes = true;
        for(int i = 0; i < list.Count - 1; i++) 
        {
            newList.Add(list[i+1] - list[i]);
            if(list[i] != 0)
                isZeroes = false;
        }
        if(isZeroes)
            return 0;
        return newList[0] - Part2(newList); //does the same as part 1 but slightly different.
    }
    
    private List<int> AddToList(string[] str) //Adding to the list each number in the file. This is for neatness
    {
        List<int> newList = new();
        foreach(string s in str)
            newList.Add(int.Parse(s));

        return newList;
    }
}
