using System.Text.RegularExpressions;
public class Day5
{
    private struct SeedData
    {
        public long Value, Range; //Values are for the seed position, Range (part 2 exclusively) is for the end of the range of seeds
        public bool Changes; //once a seed changes position, it shouldn't be able to change position again until it's on the next map
        public SeedData(long val, long range = 0, bool change = false) 
        {
            Value = val;
            Range = range;
            Changes = change;
        } //I know about setters and getters, they just didn't seem to work properly so I abandoned them this time...
    }
    public long Answer(bool part2) 
    { 
        string[] file = File.ReadAllLines("AoC5.txt"); 
        long min = Int64.MaxValue; //both parts want a minimum location
        List<SeedData> seeds = new();
        MatchCollection match = Regex.Matches(file[0],@"(\d+)"); //less regex than usual. Just parses the seeds

        if(!part2) 
        {
            for(int i = 0; i < match.Count; i++) //each individual seed
                seeds.Add(new SeedData(long.Parse(match[i].Value)));

            foreach(string line in file) 
            {
                MatchCollection newLine = Regex.Matches(line, @"(\d+)");
                if(newLine.Count != 3) //when the count == 0, it basically just means that the seeds will be moving to their next location
                { //so, that means all of them get a fresh new 'False' to the change variable!
                    ClearSeeds(seeds);
                    continue;
                }
                ChangeVal(seeds, long.Parse(newLine[0].Value), long.Parse(newLine[1].Value), long.Parse(newLine[2].Value));
            }
        }
        else
        {
            for(int i = 0; i < match.Count; i+=2) //each range of seed (from initial seed to the last seed)
                seeds.Add(new SeedData(long.Parse(match[i].Value), 
                                       long.Parse(match[i].Value) + 
                                       long.Parse(match[i+1].Value) - 1));

            foreach(string line in file)
            {
                MatchCollection newLine = Regex.Matches(line, @"(\d+)");
                if(newLine.Count != 3) 
                {
                    ClearSeeds(seeds);
                    continue;
                }
                Part2Changes(seeds, long.Parse(newLine[0].Value), long.Parse(newLine[1].Value), long.Parse(newLine[2].Value));
            }
        }
        foreach(SeedData i in seeds) //finding minimum.
            min = (long)Math.Min(i.Value, min);

        return min;
    }
    private void ChangeVal(List<SeedData> seeds, long dest, long source, long length) 
    {
        for(int i = 0; i < seeds.Count; i++) //simply checks if the seed is within the source provided by the file
        {
            if(source <= seeds[i].Value && source + length > seeds[i].Value && !seeds[i].Changes) 
            {
                seeds[i] = new SeedData(seeds[i].Value - source + dest, 0, true);
            } //yes, I used a constructor. it just changes the position of the seed to the destination
        }
    }
    private void Part2Changes(List<SeedData> seeds, long dest, long source, long length) 
    {
        int len = seeds.Count;
        for(int i = 0; i < len; i++) {
            if(seeds[i].Changes || source + length < seeds[i].Value || source > seeds[i].Range) //so the seed doesn't get changed after it has already been changed in the map
                continue; //or if the source doesn't overlap the seed, it shouldn't bother and just continues.

            if(source <= seeds[i].Value && source + length > seeds[i].Range) 
            {
                seeds[i] = new SeedData(seeds[i].Value - source + dest, seeds[i].Range - source + dest, true);
            }
            else if(source > seeds[i].Value && source + length > seeds[i].Range) //case: source begins after first seed
            {
                seeds.Add(new SeedData(dest, seeds[i].Range - source + dest, true));
                seeds[i] = new SeedData(seeds[i].Value, source - 1);
            }
            else if(source <= seeds[i].Value && source + length <= seeds[i].Range) //case: source ends before last seed
            {
                seeds.Add(new SeedData(seeds[i].Value - source + dest , length - 1 + dest, true));
                seeds[i] = new SeedData(source + length, seeds[i].Range);
            }
            else //case: both of the above
            {
                seeds.Add(new SeedData(seeds[i].Value, source - 1, false));
                seeds.Add(new SeedData(source + length, seeds[i].Range, false));
                seeds[i] = new SeedData(dest, length - 1 + dest, true);
            }
        }
    }
    private void ClearSeeds(List<SeedData> seeds) { 
        for(int i = 0; i < seeds.Count; i++)
            seeds[i] = new SeedData(seeds[i].Value, seeds[i].Range);
    } //literally just so the code looks neater. Just changes all seeds.Changes to false
}
