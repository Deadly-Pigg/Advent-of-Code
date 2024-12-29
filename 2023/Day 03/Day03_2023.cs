using System.Text.RegularExpressions;
public class Day03_2023 : Days<int>
{
    private struct Coordinates 
    {
        public int startIndex; //first index of the part number - 1 
        public int endIndex; //last index of the part number + 1
        public int partValue; //value stored in the part number

        public Coordinates(int start, int end, int val) 
        { //constructor for the struct.
            startIndex = start;
            endIndex = end;
            partValue = val;
        }
    }
    Dictionary<int, List<Coordinates>> partDetails = new(); 
     /*if you are curious exactly what's going on here, the partDetails Dictionary is formatted like this;
            Key = y coordinate/line number that was getting parsed from
            Values = refer to Coordinates struct
            */
    public override int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC3.txt"); //reading from file in the bin
        int y = 0; //aformentioned y coordinate

        if(partDetails.Count == 0) {
            foreach(string line in file) 
            { //loops through each line for the part Numbers + details
                InsertPartNo(Regex.Matches(line, @"(\d+)"), y);
                y++;
            }
        }
        
        if(part2) 
            return Part2(file);
        return Part1(file);
    }
    private int Part1(string[] file) 
    {
        List<int> symbolLoc = new(); //location of the symbols
        int sum = 0;

        for(int y = 0; y < file.Length; y++) //loops through each line for the symbols and math stuff
        { 
            MatchCollection reg = Regex.Matches(file[y], @"[^\d\.\s]"); //checks for any symbol
            foreach(Match m in reg)
                symbolLoc.Add(m.Index);

            if(symbolLoc.Count == 0) //no need to continue if the list is empty (means there's no symbols on the line that's getting read)
                continue;

            foreach(int index in symbolLoc) 
            {
                if(partDetails.ContainsKey(y-1)) //checks for part numbers above symbol(s)
                    sum += CheckNeighbours(partDetails[y-1], index);
                if(partDetails.ContainsKey(y)) //on same line as symbol(s)
                    sum += CheckNeighbours(partDetails[y], index);
                if(partDetails.ContainsKey(y+1)) //and adds them to the sum.
                    sum += CheckNeighbours(partDetails[y+1], index);
            }
            symbolLoc.Clear();
        }
        return sum;
    }
    private int Part2(string[] file) 
    {
        List<int> symbolLoc = new();
        int sum = 0;

        for(int y = 0; y < file.Length; y++) 
        { 
            MatchCollection reg = Regex.Matches(file[y], @"[*]"); //only checks for '*'
            foreach(Match m in reg)
                symbolLoc.Add(m.Index);

            if(symbolLoc.Count == 0)
                continue;

            foreach(int index in symbolLoc) //index of each individual gear instead of as a collective
            { 
                int[] sumCount = {0, 1}; //used for the amount of neighbours of the gears, and the gear ratio, respectively
                if(partDetails.ContainsKey(y-1)) //same as before
                    sumCount = GearCounter(partDetails[y-1], index, sumCount[1], sumCount[0]);
                if(partDetails.ContainsKey(y))
                    sumCount = GearCounter(partDetails[y], index, sumCount[1], sumCount[0]);
                if(partDetails.ContainsKey(y+1))
                    sumCount = GearCounter(partDetails[y+1], index, sumCount[1], sumCount[0]);
                
                if(sumCount[0] == 2) 
                    sum += sumCount[1];
            } //fun fact for part 2; at no point does any other symbol touch more than 1 part number.
            symbolLoc.Clear();
        }
        partDetails.Clear(); //just to free up memory, since I run both parts at the same time. This is to prevent memory issues
        return sum;
    }
    private int CheckNeighbours(List<Coordinates> list, int index)  //checks for if there's a neighbouring part number for each symbol
    {
        int sum = 0;
        foreach(Coordinates coords in list) 
        {
            if(coords.startIndex <= index && coords.endIndex >= index)  //checks if the symbol is located within the part number's range
                sum += coords.partValue;
        }
        return sum;
    }
    private int[] GearCounter(List<Coordinates> list, int index, int sum, int count) //same as above, but with gears + a counter
    {   
        foreach(Coordinates coords in list) 
        {
            if(coords.startIndex <= index && coords.endIndex >= index) 
            {
                sum *= coords.partValue;
                count++;
            }
        }
        return new int[] {count, sum};
    }

    private void InsertPartNo(MatchCollection reg, int y) //check earlier comment
    { 
        List<Coordinates> newList = new();
        foreach(Match m in reg) 
            newList.Add(new Coordinates(m.Index-1, m.Index + m.Length, int.Parse(m.Value)));
            //newList.Add(new Coordinates(startIndex, endIndex, partValue));
        partDetails.Add(y, newList);
    }
}
