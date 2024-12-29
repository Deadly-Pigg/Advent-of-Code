public class Day15 : Days<long>
{
    private struct FocusPower
    {
        public string Data;
        public int Lens;
        public int Box;
        public FocusPower(string d, int l, int b)
        {
            Data = d;
            Lens = l;
            Box = b;
        }
    }
    HashSet<FocusPower> hashmap = new();
    Dictionary<string, FocusPower> addedVals = new();
    public override long Answer(bool part2) 
    { 
        long sum = 0;
        string[] file = File.ReadAllText("AoC15.txt").Split(',');
        
        if(!part2)
        {
            foreach(string str in file)
                sum += Part1(str);
        }
        else
        {
            int lens = 0;
            foreach(string str in file)
                lens = Part2(str, lens);

            Dictionary<int, int> newDict = new();
            foreach(FocusPower focus in hashmap)
            {
                if(!newDict.ContainsKey(focus.Box))
                    newDict[focus.Box] = 0;
                newDict[focus.Box]++;
                Console.WriteLine($"name:{focus.Data}, box:{focus.Box}, slot:{newDict[focus.Box]}, lens:{focus.Lens}");
                sum += (focus.Box+1) * focus.Lens * newDict[focus.Box];
            }
            Console.WriteLine(sum);
        }

        return sum;
    }
    private int Part2(string input, int lens)
    {
        if(input[^1] == '-')
        {
            string tempDel = input[..(input.Length-1)]; 
            if(!addedVals.ContainsKey(tempDel))
                return lens;
            
            FocusPower removeTemp = addedVals[tempDel];
            HashSet<FocusPower> tempHash = new();
            hashmap.Remove(removeTemp);

            foreach(FocusPower f in hashmap)
                tempHash.Add(f);

            hashmap = tempHash;
            
            addedVals.Remove(tempDel);

            return lens;
        }
        
        string tempData = input[..(input.Length - 2)];
        int newLens = input[^1] - '0';
        int box = Part1(tempData);
        FocusPower addTemp = new FocusPower(tempData, newLens, box);
        
        if(!addedVals.ContainsKey(tempData))
        {
            hashmap.Add(addTemp);
            addedVals.Add(tempData, addTemp);
        }
        else
        {
            FocusPower changeTemp = addedVals[tempData];
            FocusPower newFP = new FocusPower(changeTemp.Data, newLens, changeTemp.Box);
            hashmap.Remove(changeTemp);
            hashmap.Add(newFP);
            addedVals[tempData] = newFP;
        }

        return newLens;
    }
    private int Part1(string input) 
    { 
        int sum = 0;
        foreach(char c in input)
        {
            sum += c;
            sum *= 17;
            sum %= 256; 
        }
        return sum;
    }
}
