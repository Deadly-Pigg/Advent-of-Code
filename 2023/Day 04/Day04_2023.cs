using System.Text.RegularExpressions;
public class Day04_2023 : Days<int>
{
    Dictionary<int, int> dict = new(); //for part 2
    Dictionary<int, HashSet<int>> hashDict = new(); //for storing winning numbers of each card
    List<MatchCollection> regex1 = new(); //stores numbers before the | (winning)
    List<MatchCollection> regex2 = new(); //stores numbers after the | (numbers we have)
    public override int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC4.txt"); //VictorVictini is col. This is my slowest solution yet for some odd reason. Might be the regex.
        
        if(hashDict.Count == 0) //if the hashDict already contains values, then there's no need to re-add them.
        { //this applies for ALL global variables in this class
            for(int i = 0; i < file.Length; i++) 
            {
                string newLine = Regex.Replace(file[i], @".+:", ""); //gets rid of the 'Card #:' part of the line
                regex1.Add(Regex.Matches(newLine, @"(?<! \|.*)(\d{1,})+")); 
                regex2.Add(Regex.Matches(newLine, @"(?<= \|.*)(\d{1,})+")); 
                HashSet<int> hash = new HashSet<int>(); 

                foreach(Match m in regex1[i]) 
                    hash.Add(int.Parse(m.Value));

                hashDict.Add(i, hash);
                dict[i] = 1; // This is useful since part 2 asks for you to count all the scratch cards and they can get exponentially large.
            }
        }

        return part2 ? Part2(file) : Part1(file);
    }

    private int Part1(string[] file) 
    {
        int sum = 0;

        for(int i = 0; i < file.Length; i++) 
        {
            int count = 0;
            foreach(Match m in regex2[i]) 
            {
                if(hashDict[i].Contains(int.Parse(m.Value))) //just checks if the numbers you have are winning
                    count += count == 0 ? 1 : count;
            }
            sum += count;
        }
        return sum;
    }

    private int Part2(string[] file) 
    {
        int sum = 0;

        for(int i = 0; i < file.Length; i++) 
        {
            int j = 1;
            foreach(Match m in regex2[i]) //scratch-card counter (it adds the total amount of the initial scratchcard to the following scratchcard(s))
            {
                if(hashDict[i].Contains(int.Parse(m.Value))) //scratch-card counter (it adds the total amount of the initial scratchcard to the following scratchcard(s))
                { 
                    dict[j+i] += dict[i];
                    j++;
                }
            }
            sum += dict[i];
        }
        return sum;
    }
}
