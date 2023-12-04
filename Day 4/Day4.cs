using System.Text.RegularExpressions;
public class Day4
{
    public int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC4.txt"); //VictorVictini is col.
        Dictionary<int, int> dict = new Dictionary<int, int>(); //for part 2
        int sum = 0;

        if(part2) 
        {
            for(int i = 0; i < file.Length; i++) 
            { //initialising values in the dictionary. This is useful since part 2 asks for you to count all the scratch cards
                dict[i] = 1; //and they can get exponentially large.
            }
        }

        for(int i = 0; i < file.Length; i++) 
        {
            string newLine = Regex.Replace(file[i], @".+:", ""); //gets rid of the 'Card #:' part of the line
            MatchCollection regex1 = Regex.Matches(newLine, @"(?<! \|.*)(\d{1,})+"); //stores numbers before the | (winning)
            MatchCollection regex2 = Regex.Matches(newLine, @"(?<= \|.*)(\d{1,})+"); //stores numbers after the | (numbers we have)
            HashSet<int> hash = new HashSet<int>(); //for storing winning numbers
            foreach(Match m in regex1) 
            {
                hash.Add(int.Parse(m.Value));
            }
            int count = 0;
            if(part2) 
            {
                int j = 1;
                foreach(Match m in regex2) 
                {
                    //no need to worry about a key not existing; the description states 'Cards will never make you copy a card past the end of the table.'
                    if(hash.Contains(int.Parse(m.Value)))                     
                    { //scratch-card counter (it adds the total amount of the initial scratchcard to the following scratchcard(s))
                        dict[j+i] += dict[i];
                        j++;
                    }
                }
                sum += dict[i];
                continue;
            }
            foreach(Match m in regex2) 
            {
                if(hash.Contains(int.Parse(m.Value))) //just checks if the numbers you have are winning
                { //point counter 
                    count += count == 0 ? 1 : count;
                }
            }
            sum += count;
        }
        return sum;
    }
}
