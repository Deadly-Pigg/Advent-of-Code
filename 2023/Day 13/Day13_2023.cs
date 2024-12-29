public class Day13 : Days<int>
{
    List<List<string>> images = new();
    public override int Answer(bool part2) 
    {
        if(!part2)
            Parse(); //puts individual boards into seperate string arrays in the 'images' list

        int sum = 0;

        for(int i = 0; i < images.Count; i++)
            sum += Reflection(images[i].ToArray(), part2);

        if(part2)
            images.Clear(); //memory saving purposes.

        return sum;
    }
    public int Reflection(string[] board, bool part2) //checks for the index of reflection
    {
        for(int i = 1; i < board[0].Length; i++)    //for the columns. E.g:
        {                                           //     ><
            if(SymmetricalColumn(i, board, part2))  //  #.#..#.#
                return i;                           //  #..##..#
        }                                           //2nd for loop is for the rows.

        for(int i = 1; i < board.Length; i++)       //part2 is detecting smudges.
        {                                           //since there can only ever be one smudge,
            if(SymmetricalRow(i, board, part2))     //it checks if a specific reflection point has exactly 1 smudge.
                return i*100;
        }

        throw new Exception("no reflection found. Above board was used"); //was mainly used for debugging, but checks for valid inputs
    }
    public bool SymmetricalColumn(int index, string[] board, bool part2) //column checking
    {         //SymmetricalVictorVictiniumn
        int smudges = 0;
        for(int i = 0; i < board.Length; i++)
        {
            int left = index-1;
            int right = index;
            string temp = board[i];
            while(left >= 0 && right < board[0].Length)
            {
                if(temp[left] != temp[right])
                {
                    if(!part2)
                        return false;
                    smudges++;
                }
                left--;
                right++;
            }
        }
        return part2 ? smudges == 1 : true;
    }
    public bool SymmetricalRow(int index, string[] board, bool part2) //row checking
    {
        int smudges = 0;
        for(int i = 0; i < board[0].Length; i++)
        {
            int left = index-1;
            int right = index;
            while(left >= 0 && right < board.Length)
            {
                if(board[left][i] != board[right][i])
                {
                    if(!part2)
                        return false;
                    smudges++;
                }
                left--;
                right++;
            }
        }
        return part2 ? smudges == 1 : true;
    }
    public void Parse() //reads from file then just puts into list.
    {
        string[] file = File.ReadAllLines("AoC13.txt"); 
        List<string> newImg = new();

        foreach(string line in file)
        {
            if(line.Length <= 1)
            {
                images.Add(new List<string>(newImg));
                newImg.Clear();
                continue;
            }
            newImg.Add(line);
        }
        images.Add(new List<string>(newImg));
    }
}
