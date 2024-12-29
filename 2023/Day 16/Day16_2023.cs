public class Day16 : Days<int>
{
    public override int Answer(bool part2) 
    {
        string[] file = File.ReadAllLines("AoC16.txt");
        if(!part2)
            return BeamPath(file, 0,0,1,new HashSet<(int, int, int)>(), new HashSet<(int,int)>());

        int max = 0;

        for(int y = 0; y < file.Length; y++)
            max = Math.Max(max, BeamPath(file, 0,y,1,new HashSet<(int, int, int)>(), new HashSet<(int,int)>()));

        for(int y = 0; y < file.Length; y++)
            max = Math.Max(max, BeamPath(file, file[0].Length-1,y,3,new HashSet<(int, int, int)>(), new HashSet<(int,int)>()));

        for(int x = 0; x < file[0].Length; x++)
            max = Math.Max(max, BeamPath(file, x,0,2,new HashSet<(int, int, int)>(), new HashSet<(int,int)>()));

        for(int x = 0; x < file[0].Length; x++)
            max = Math.Max(max, BeamPath(file, x,file.Length-1,0,new HashSet<(int, int, int)>(), new HashSet<(int,int)>()));
        
        return max;
    }
    public int BeamPath(string[] file, int x, int y, int d, HashSet<(int,int,int)> currPath, HashSet<(int,int)> visited)
    {
        if(x < 0 || x >= file[0].Length || y < 0 || y >= file.Length || currPath.Contains((x,y,d)))
            return visited.Count;

        visited.Add((x,y));
        currPath.Add((x,y,d));
        while(file[y][x] == '.')
        {
            switch(d)
            {
                case 0: y--; break;
                case 1: x++; break;
                case 2: y++; break;
                case 3: x--; break;
            }

            if(x < 0 || x >= file[0].Length || y < 0 || y >= file.Length || currPath.Contains((x,y,d)))
                return visited.Count;

            visited.Add((x,y));
            currPath.Add((x,y,d));
        }
        switch(file[y][x])
        {
            case '|':         
            {
                if(d != 0)
                    BeamPath(file, x, y+1, 2, currPath, visited);
                if(d != 2)
                    BeamPath(file, x, y-1, 0, currPath, visited);
                return visited.Count;
            }
            case '-':
            {
                if(d != 3)
                    BeamPath(file, x+1, y, 1, currPath, visited);
                if(d != 1)
                    BeamPath(file, x-1, y, 3, currPath, visited);
                return visited.Count;
            }
            case '\\' :
            {
                if(d == 3 || d == 0)
                    d = d == 3 ? 0 : 3;
                else
                    d = d == 1 ? 2 : 1;
                break;
            }
            case '/' :
            {
                if(d == 2 || d == 3)
                    d = d == 3 ? 2 : 3;
                else
                    d = d == 1 ? 0 : 1;
                break;
            }
        }
        switch(d)
        {
            case 0: y--; break;
            case 1: x++; break;
            case 2: y++; break;
            case 3: x--; break;
        }
        return BeamPath(file, x, y, d, currPath, visited);
    }
}
