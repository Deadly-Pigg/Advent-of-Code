using System.Text;
using System.Text.RegularExpressions;
public class Day02_2024 : Days<long>
{
    int[][] reports = new int[0][];
    public override long Answer(bool part2) 
    {
        if(!part2)
            Parse();

        return ReportHandler(part2);
    }

    public int ReportHandler(bool part2)
    {
        int count = 0; //count of safe reports

        for(int i = 0; i < reports.Length; i++) //for each report
        {
            if(reports[i].Length <= 2) //if report is at most 2 elements, it is safe. (Note: this case is never met in the input provided)
            {
                count++;
                continue;
            }

            bool isIncreasing = reports[i][0] > reports[i][1]; //stores report order
            
            if(!part2)
                count += Part1(reports[i], isIncreasing);
            else
                count += Part2(reports[i]);
        }
        return count;
    }

    public int Part1(int[] report, bool isIncreasing)
    {
        for(int i = 1; i < report.Length; i++)
        {
            if(!CheckSafety(report[i-1], report[i], isIncreasing))
                return 0;
        }
        return 1;
    }

    public int Part2(int[] report)
    {
        for(int j = 0; j <= report.Length; j++)
        {
            List<int> newInput = new();
            for(int k = 0; k < report.Length; k++)
            {
                if(j != k)
                    newInput.Add(report[k]);
            }
            if(IsSafe(newInput))
                return 1;
        }
        return 0;
    }

    private bool IsSafe(List<int> reports)
    {
        bool safe = true;
        bool isIncreasing = reports[0] > reports[1];
        for(int j = 1; safe && j < reports.Count; j++)
            safe = CheckSafety(reports[j-1], reports[j], isIncreasing);
        return safe;
    }

    private bool CheckSafety(int report1, int report2, bool isIncreasing)
    {
        return report1 != report2 && 
            Math.Abs(report1 - report2) <= 3 
            && (isIncreasing ^ report1 < report2);
    }

    private void Parse()
    {
        string[] file = File.ReadAllLines("02_2024.txt"); //reading from the file content
        reports = new int[file.Length][];

        for(int i = 0; i < file.Length; i++) //parsing each line in file
        {
            var matches = Regex.Matches(file[i], @"\d+");
            reports[i] = new int[matches.Count];
            for(int j = 0; j < matches.Count; j++)
                reports[i][j] = Helpers_2024.NumberParse(matches[j], i);
        }
    }
}
