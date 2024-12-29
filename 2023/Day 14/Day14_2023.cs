using System.Text.RegularExpressions;
public class Day14 : Days<long>
{
    Dictionary<int, List<int>> old = new();
    Dictionary<string[], int> storedVals = new();
    public override long Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC14.txt");
        
        if(!part2)
        {
            file = North(file);
            return SumUp(file);
        }
        else
        {
            int iterations = 0;

            while(iterations < 300)
            {
                if(iterations > 130)
                {
                    int temp = SumUp(file);
                    if(!old.ContainsKey(temp))
                        old.Add(temp, new List<int>());
                    old[temp].Add(iterations);
                }
                
                file = North(file);
                file = West(file);
                file = South(file);
                file = East(file);
                iterations++;
            }
            return BringDown((int)1e9);
        }

    }
    private int BringDown(int num)
    {
        int diff = 0;
        foreach(List<int> lst in old.Values) //I am quite lazy, but this just gets the difference of cycles between occurences.
        {
            diff = lst[1] - lst[0];
            break;
        }

        foreach(int val in old.Keys)
        {
            int temp = old[val][0];
            if((num - temp) % diff == 0)
                return val;
        }
        return -1;
    }
    private int SumUp(string[] file)
    {
        if(storedVals.ContainsKey(file))
            return storedVals[file];

        string[]? newFile = file.Clone() as string[];

        if(newFile == null)
            return -1;

        int sum = 0;
        for(int i = 0; i < file.Length; i++)
            sum += (file.Length - i) * Regex.Matches(file[i], @"O").Count();
        storedVals.Add(newFile, sum);
        return sum;
    }
    private string[] North(string[] file) //moves rocks up
    {
        for(int col = 0; col < file[0].Length; col++)
        {
            int oCount = 0;
            for(int row = file.Length-1; row >= 0; row--)
            {
                if(file[row][col] == '.')
                    continue;

                if(file[row][col] == 'O')
                {
                    file[row] = ReplaceAt(file[row], col, '.');
                    oCount++;
                    continue;
                }
                for(int i = row+1; i < row+oCount+1; i++)
                    file[i] = ReplaceAt(file[i], col, 'O');

                oCount = 0;
            }
            for(int i = 0; i < oCount; i++)
                file[i] = ReplaceAt(file[i], col, 'O');
        }

        return file;
    }
    private string[] South(string[] file)
    {
        for(int col = 0; col < file[0].Length; col++)
        {
            int oCount = 0;
            for(int row = 0; row < file.Length; row++)
            {
                if(file[row][col] == '.')
                    continue;

                if(file[row][col] == 'O')
                {
                    file[row] = ReplaceAt(file[row], col, '.');
                    oCount++;
                    continue;
                }
                for(int i = row-oCount; i < row; i++)
                    file[i] = ReplaceAt(file[i], col, 'O');

                oCount = 0;
            }
            for(int i = file.Length-oCount; i < file.Length; i++)
                    file[i] = ReplaceAt(file[i], col, 'O');
        }

        return file;
    }
    private string[] East(string[] file)
    {
        for(int row = 0; row < file.Length; row++)
        {
            int oCount = 0;
            for(int col = 0; col < file[0].Length; col++)
            {
                if(file[row][col] == '.')
                    continue;

                if(file[row][col] == 'O')
                {
                    file[row] = ReplaceAt(file[row], col, '.');
                    oCount++;
                    continue;
                }
                for(int i = col-oCount; i < col; i++)
                    file[row] = ReplaceAt(file[row], i, 'O');

                oCount = 0;
            }
            for(int i = file[0].Length-oCount; i < file[0].Length; i++)
                file[row] = ReplaceAt(file[row], i, 'O');
        }

        return file;
    }
    private string[] West(string[] file)
    {
       for(int row = 0; row < file.Length; row++)
        {
            int oCount = 0;
            for(int col = file[0].Length-1; col >= 0; col--)
            {
                if(file[row][col] == '.')
                    continue;

                if(file[row][col] == 'O')
                {
                    file[row] = ReplaceAt(file[row], col, '.');
                    oCount++;
                    continue;
                }
                for(int i = col+oCount; i > col; i--)
                    file[row] = ReplaceAt(file[row], i, 'O');

                oCount = 0;
            }
            for(int i = oCount-1; i >= 0; i--)
                    file[row] = ReplaceAt(file[row], i, 'O');
        }

        return file;
    }
    private string ReplaceAt(string str, int index, char replacement)
    {
        char[] charray = str.ToCharArray();
        charray[index] = replacement;
        return new string(charray);
    }
}