public class Day12 : Days<long>
{
    List<int[]> sizeList = new (); //for the 3,1,2 stuff
    List<string> inputList = new (); //for the ###??..?# stuff
    Dictionary<int, string> newInput = new();
    Dictionary<int, string> betterInput = new();
    public override long Answer(bool part2) 
    { 
        if(!part2) //57,095,636,756,178 is TOO HIGH 57,095,634,897,948 too
            Parse();
        
        long sum = 0;
        for(int i = 0; i < inputList.Count; i++)
        {
            Console.Write(inputList[i] + ", [" + String.Join(",",sizeList[i]) + "]:  ");
            long temp = part2 ? Part2(inputList[i], sizeList[i], i) : Part1(inputList[i], sizeList[i], i);
            if(betterInput.ContainsKey(i))
                Console.Write(" also, this exists: " + betterInput[i] + "        ");
            Console.WriteLine(temp);
            sum += temp;
        }
        Console.WriteLine("\n\n\n");
        return sum;
    }
    /*
    private int Part2(int index)
    {
        int[] newList = new int[sizeList[index].Length * 5];
        int[] temp = sizeList[index];
        string[] strings = new string[5];

        for(int i = 0; i < newList.Length; i++)
            newList[i] = temp[i % temp.Length];
        
        for(int i = 0; i < 5; i++)
            strings[i] = inputList[index];

        string input = String.Join("?",strings);

        Console.Write(input + ", [" + String.Join(",",newList) + "]:  ");

        return Recurse(newList, input, 0);
    }*/

    private long Part2(string input, int[] newList, int index)
    {
        string otherInput = input;
        if(betterInput.ContainsKey(index))
            otherInput = betterInput[index];
            
        string input1 = input + (otherInput[0] == '#' ? '.' : '?');
        string input2 = (otherInput[^1] == '#' ? '.' : '?') + input + (otherInput[0] == '#' ? '.' : '?');
        string input3 = (otherInput[^1] == '#' ? '.' : '?') + input;
        return Part1(input1, newList, -1) * (long)Math.Pow(Part1(input2, newList, -1), 3) * Part1(input3, newList, -1);
    }
    private int Part1(string input, int[] newList, int index) //now for the fancy stuff. I'll bruteforce for now, then do the math later.
    { //my idea revolves around multiplication of triangle numbers. (triangle numbers are factorials but addition: n,n-1,n-2..2+1), but I am unsure of how to do it with groups > 2
        int sum = Recurse(newList, input, 0, index);
        if(sum == 1 && index != -1)
        {
            betterInput.Add(index, newInput[index]);
        }
        return sum;
    }
    private int Recurse(int[] newList, string input, int i, int index)
    {
        for(; i < input.Length; i++)
        {
            if(input[i] == '?')
                return Recurse(newList, ReplaceAt(input, i, '#') ,i+1, index) + Recurse(newList, ReplaceAt(input, i, '.'),i+1, index);
        }
        return Validate(newList, input, index);
    }
    private int Validate(int[] newList, string input, int index)
    {
        int j = 0;
        bool isGroup = false;
        int[] count = new int[newList.Length];
        input += ".";

        for(int i = 0; i < count.Length; i++)
            count[i] = newList[i];

        for(int i = 0; i < input.Length; i++)
        {
            if(input[i] == '#')
            {
                if(j == newList.Length)
                    return 0;
                count[j]--;
                isGroup = true;
            }
            else if(isGroup)
            {
                if(count[j] != 0)
                    return 0;
                isGroup = false;
                j++;
            }
        }

        if(j != count.Length)
            return 0;

        if(!newInput.ContainsKey(index))
            newInput.Add(index, input[..(input.Length - 1)]);
        return 1;
    }
    private string ReplaceAt(string str, int index, char replacement)
    {
        char[] charray = str.ToCharArray();
        charray[index] = replacement;
        return new string(charray);
    }
    private void Parse()
    {
        string[] file = File.ReadAllLines("AoC12.txt");

        foreach(string line in file) //parsing the input; splitting the lines and sizes into each list
        {
            string[] str = line.Split(" ");
            inputList.Add(str[0]);

            string[] sizeStr = str[1].Split(","); //simply just getting the sizes into arrays.
            int[] sizeArr = new int[sizeStr.Length];
            for(int i = 0; i < sizeStr.Length; i++)
                sizeArr[i] = int.Parse(sizeStr[i]);
            sizeList.Add(sizeArr);
        }
    }
}
