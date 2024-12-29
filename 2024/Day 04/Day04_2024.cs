using System.Text;
using System.Text.RegularExpressions;
public class Day04_2024 : Days<int>
{
    string[] file = File.ReadAllLines("04_2024.txt");
    int[][] directions = new int[][] { //{x,y}
        new int[] {0,-1}, //N
        new int[] {1,-1}, //N-E
        new int[] {1,0}, //E
        new int[] {1,1}, //S-E
        new int[] {0,1}, //S
        new int[] {-1,1}, //S-W
        new int[] {-1,0}, //W
        new int[] {-1,-1} //N-W
    };
    bool[,] visited;
    bool visualise = false;
    public override int Answer(bool part2) 
    {
        visited = new bool[file.Length, file[0].Length];
        int count = 0;
        for(int x = 0; x < file.Length; x++)
        {
            for(int y = 0; y < file[0].Length; y++)
            {
                if(!part2)
                    count += Part1(x,y);
                else
                    count += Part2(x,y);
            }
        }
        if(visualise)
            Visualise();
        return count;
    }
    public int Part1(int x, int y)
    {
        if('X' != file[x][y])
            return 0;
        
        int sum = 0;
        for(int i = 0; i < directions.Length; i++)
        {
            int tempX = x;
            int tempY = y;
            int curr = 1;
            for(; curr < 4; curr++)
            {
                tempX += directions[i][0];
                tempY += directions[i][1];
                if(tempX < 0 || tempY < 0 || tempX == file.Length || tempY == file[0].Length || file[tempX][tempY] != "XMAS"[curr])
                    break;
            }
            if(curr != 4)
                continue;
            sum++;
            if(visualise)
                p1_Visualiser(x,y,i);
        }
        return sum;
    }

    public int Part2(int x, int y)
    {
        if(file[x][y] != 'A' || x == 0 || y == 0 || x == file.Length-1 || y == file[0].Length-1)
            return 0;
        
        int mC = 0;
        int sC = 0;
        for(int i = 1; i < directions.Length; i+=2)
        {
            char temp = file[x+directions[i][0]][y+directions[i][1]];

            if(temp == 'M')
                mC++;
            else if(temp == 'S')
                sC++;
            else
                return 0;

            int opposite = (i+4)%directions.Length;
            if(temp == file[x+directions[opposite][0]][y+directions[opposite][1]])
                return 0;
        }

        if(sC != 2 || mC != 2)
            return 0;

        if(visualise)
        {
            visited[x,y] = true;
            for(int i = 1; i < directions.Length; i+=2)
                visited[x+directions[i][0],y+directions[i][1]] = true;
        }
        return 1;
    }

    public void p1_Visualiser(int x, int y, int dir)
    {
        visited[x,y] = true;
        for(int i = 1; i < 4; i++)
        {
            x += directions[dir][0];
            y += directions[dir][1];
            visited[x,y] = true;
        }
    }
    public void Visualise()
    {
        Console.WriteLine("");
        for(int x = 0; x < file.Length; x++)
        {
            for(int y = 0; y < file[0].Length; y++)
                if(visited[x,y])
                    Console.Write(file[x][y]);
                else
                    Console.Write(".");
            Console.WriteLine();
        }
    }
}
