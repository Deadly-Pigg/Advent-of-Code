using System.Text.RegularExpressions;
public class Day11 : Days<long>
{
    HashSet<int> ExpandedRows = new();
    HashSet<int> ExpandedCols = new();
    List<int[]> list = new();
    public override long Answer(bool part2) 
    { 
        if(!part2)
            Parse();

        int[][] points = new int[list.Count][];
        
        if(part2)
        {
            points = Expansion(1000000 - 1); //-1 is because every 1 empty space == 1 million. doing just 1 million means 1 empty space becomes 1 million + 1
            ExpandedRows.Clear();
            ExpandedCols.Clear();
            list.Clear(); //freeing up memory
        }
        else
            points = Expansion(2 - 1);

        return DistanceFinder(points);
    }
    public void Parse() //converting input into correct format for performing operations
    {
        string[] file = File.ReadAllLines("AoC11.txt");

        for(int x = 0; x < file[0].Length; x++)
                ExpandedRows.Add(x);

            for(int y = 0; y < file.Length; y++)
            {
                ExpandedCols.Add(y);
                for(int x = 0; x < file[0].Length; x++)
                {
                    if(file[y][x] == '#')
                    {
                        ExpandedRows.Remove(x);
                        ExpandedCols.Remove(y);
                        list.Add(new int[] {x,y});
                    }
                }
            }
    }
    public int[][] Expansion(int expandAmount)
    {
        int expansion = 0;
        int[][] points = new int[list.Count][];
        
        HashSet<int> ExpC = new HashSet<int>(ExpandedCols); //this somehow works as a deep copy.
        HashSet<int> ExpR = new HashSet<int>(ExpandedRows);

        for(int i = 0; i < points.Length; i++) //making a deep-copy of list and adding it to points. points = list.ToArray(); just makes points a pointer for list
        {
            points[i] = new int[2];
            points[i][0] = list[i][0];
            points[i][1] = list[i][1];
        }

        for(int i = 0; i < points.Length; i++) //expanding galaxies downwards if there is empty space
        {
            foreach(int y in ExpC)
            {
                if(points[i][1] > y)
                {
                    ExpC.Remove(y);
                    expansion += expandAmount;
                }
            }
            points[i][1] += expansion;
        }

        expansion = 0;
        MergeSort(points, 0, points.Length - 1); //sorting points

        for(int i = 0; i < points.Length; i++)
        {
            foreach(int x in ExpR)
            {
                if(points[i][0] > x)
                {
                    ExpR.Remove(x);
                    expansion += expandAmount;
                }
            }
            points[i][0] += expansion;
        }
        return points;
    }
    public long DistanceFinder(int[][] points) //sum of distance between ALL pairs of galaxies
    {
        long sum = 0;
        for(int i = 0; i < points.Length; i++)
        {
            for(int j = i+1; j < points.Length; j++)
            {
                sum += Math.Abs(points[i][0] - points[j][0]) + Math.Abs(points[i][1] - points[j][1]);
            }
        }
        return sum;
    }

    private void MergeSort(int[][] points, int start, int end) //food
    { 
        if(start >= end)
            return;
        int mid = start + (end - start) / 2;
        MergeSort(points, start, mid);
        MergeSort(points, mid+1, end);
        Merge(points, start, mid, end);
    }
    private void Merge(int[][] points, int start, int mid, int end)
    {
        int leftLen = mid - start + 1;
        int rightLen = end - mid;

        int[][] left = new int[leftLen][];
        int[][] right= new int[rightLen][];

        int i, leftInd = 0, rightInd = 0;

        for(i = 0; i < leftLen; i++)
            left[i] = points[start + i];

        for(i = 0; i < rightLen; i++)
            right[i] = points[mid + i + 1];
        
        i = start;

        while(leftInd < leftLen && rightInd < rightLen)
        {
            if(left[leftInd][0] <= right[rightInd][0])
                points[i++] = left[leftInd++];
            else
                points[i++] = right[rightInd++];
        }

        while(leftInd < leftLen)
            points[i++] = left[leftInd++];

        while(rightInd < rightLen)
            points[i++] = right[rightInd++];
    }
}
