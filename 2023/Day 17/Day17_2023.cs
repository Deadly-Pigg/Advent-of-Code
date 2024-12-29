public class Day17 : Days<int>
{
    public struct Coords 
    {
        public int X;
        public int Y;
    
        public Coords(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    int shortest = 0;
    HashSet<Coords> bestPath = new();
    int[][] heatMap;
    Dictionary<(Coords,int), int> prevPath = new();
    bool found = false;
    public override int Answer(bool part2) 
    {
        if(part2)
            return 0;
        string[] file = File.ReadAllLines("AoC17.txt");
        shortest = 9 * file.Length * file[0].Length; //setting up pathfinding. All paths are shorter than this. (Int32.MaxValue is good too) )
        heatMap = new int[file.Length][];

        for(int i = 0; i < file.Length; i++)
        {
            heatMap[i] = new int[file[0].Length];
            Array.Fill(heatMap[i], 0);
        }
        
        PathFind(file, new Coords(0,0), -file[0][0] + '0', new HashSet<Coords>(), -1, 1, file.Length + file[0].Length * 4);

        for(int y = 0; y < file.Length ;y++)
        {
            for(int x = 0; x < file[0].Length; x++)
            {
                if(bestPath.Contains(new Coords(x,y)))
                    Console.Write("#");
                else
                    Console.Write(file[y][x]);
            }
            Console.WriteLine();
        }

        for(int y = 0; y < file.Length ;y++)
        {
            for(int x = 0; x < file[0].Length; x++)
            {
                Console.Write(heatMap[y][x] + "\t");
            }
            Console.WriteLine(":" + y);
        }
        int sum = 0;

        foreach(int[] val in heatMap)
            sum += val.Sum();
        Console.WriteLine("total times: " + sum);

        return shortest + file[^1][^1] - '0';
    }

    public void Print(HashSet<(int,int)> path, string[] file)
    {
        for(int y = 0; y < file.Length ;y++)
        {
            for(int x = 0; x < file[0].Length; x++)
            {
                if(bestPath.Contains(new Coords(x,y)))
                    Console.Write("#");
                else
                    Console.Write(file[y][x]);
            }
            Console.WriteLine();
        }
    }

    /*Idea for pathfinding:
    * So, what I will do is calculate the cheapest node to traverse to next, regardless of the path it's taking. This is a bit brute-forcy,
    * But this will allow me to traverse a node at mode 4 times. I will terminate the search once the bottom-right node is reached.
    * This will use a PriorityQueue to store the current paths that have been traversed, and a running cost.
    * A naive impletementation would require every node to check every neighbour, regradless of if it's been found already or not. 
    * This is not feasible, so I should store it in a list-type structure, or have all the traversed nodes in a global hash-set.
    * Debugging would be a pain though, so I'd opt for the linked-list structure as well.
    * The next issue comes with the 'at most 3 of the same direction' traversed. I suppose I can make the 4th of the same direction cost 2^32 - 1 to combat this.
    * And as such, I can now implement this... tomorrow.
    */
    
    public void PathFind(string[] file, Coords xy, int pathLen, HashSet<Coords> travelled, int d, int time, int h)
    {
        if(!(xy.X >= 0 && xy.Y >= 0 && xy.X < file[0].Length && xy.Y < file.Length) || travelled.Contains(xy) || pathLen + h>= shortest || time >= 4)
            return; //terminates recursion if the path goes out-of-bounds.

        if(xy.X == file[0].Length-1 && xy.Y == file.Length-1) //if it reaches the end destination
        {
            if(pathLen < shortest)
            {
                Console.WriteLine(pathLen);
                shortest = pathLen;
                bestPath = travelled;
            }
            return;
        } 

        heatMap[xy.Y][xy.X]++; //heatmap for knowing how many times a specific index was traversed. You can ignore this.

        travelled.Add(xy);
        if(prevPath.ContainsKey((xy,time)) && prevPath[(xy,time)] < pathLen)
            return;
        prevPath[(xy,time)] = pathLen;

        int currPoint = file[xy.Y][xy.X] - '0';
        h = Math.Abs(xy.X - file[0].Length) + Math.Abs(xy.Y - file.Length);
        
        PathFind(file, new Coords(xy.X+1,xy.Y), pathLen + currPoint, new HashSet<Coords>(travelled), 1, 1 + (d == 1 ? time : 0),h);
        PathFind(file, new Coords(xy.X,xy.Y+1), pathLen + currPoint, new HashSet<Coords>(travelled), 2, 1 + (d == 2 ? time : 0),h);
        PathFind(file, new Coords(xy.X-1,xy.Y), pathLen + currPoint, new HashSet<Coords>(travelled), 3, 1 + (d == 3 ? time : 0),h);
        PathFind(file, new Coords(xy.X,xy.Y-1), pathLen + currPoint, new HashSet<Coords>(travelled), 0, 1 + (d == 0 ? time : 0),h);

    }
    public bool CompareOldPath(string[] file, Coords xy, int value, int time, int pathLen)
    {
        Coords testCoords = new Coords(xy.X + 9-value, xy.Y + 9-value);

        for(int x = xy.X; x < testCoords.X; x++)
        {
            if(prevPath.ContainsKey((new Coords(x, testCoords.Y), time)) && prevPath[(new Coords(x, testCoords.Y), time)] < pathLen + x + value * 30)
                return true;
        }
        for(int y = xy.Y; y < testCoords.Y; y++)
        {
            if(prevPath.ContainsKey((new Coords(testCoords.X, y), time)) && prevPath[(new Coords(testCoords.X, y), time)] < pathLen + y + value * 30) 
                return true;
        }

        return false;
    }
}