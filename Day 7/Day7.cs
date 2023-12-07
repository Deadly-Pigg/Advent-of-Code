using System.Text.RegularExpressions;
public class Day7 : Days<long>
{
    public struct CardData
    {
        public int Worth;
        public int Card;
        public CardData(int worth, int card)
        {
            Worth = worth;
            Card = card;
        }
    }
    Dictionary<int, List<CardData>> handTypes = new();
    Dictionary<char, int> values = new Dictionary<char, int>()
        {
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'J', 11},
            {'Q', 12},
            {'K', 13},
            {'A', 14}
        }; //assigning worth to cards

    public override long Answer(bool part2) 
    { 
        if(part2)
            values['J'] = 1; //part 2 states that the value of the 'Joker' card should be the lowest
        long sum = 0;
        int rank = 1;

        string[] file = File.ReadAllLines("AoC7.txt"); 

        if(handTypes.Count != 0)
            handTypes.Clear(); //need to ensure the dictionary is clear since both parts are executed in conjunction

        Parse(file, part2);
        
        for(int i = 0; i < 7; i++) {
            List<CardData> dict = handTypes[i];
            foreach(CardData value in dict)
            {
                sum += rank * value.Worth;
                rank++;
            }
        }

        return sum;
    }

    private void Parse(string[] file, bool part2) 
    {
        List<CardData>[] cards = new List<CardData>[7];
        for(int i = 0; i < 7; i++)
            cards[i] = new List<CardData>();

        for(int i = 0; i < file.Length; i++) 
        {
            MatchCollection regex = Regex.Matches(file[i], @"^\w+(?! ).|\d+$");
            int hand = HandType(regex[0].Value, part2);
            cards[hand].Add(new CardData(int.Parse(regex[1].Value), AssignValues(regex[0].Value)));
        }

        for(int i = 0; i < 7; i++)
        {
            MergeSort(cards[i], 0, cards[i].Count-1);
            handTypes.Add(i, cards[i]);
        }
    }

    private void MergeSort(List<CardData> dict, int start, int end)
    {
        if(start >= end)
            return;
        int mid = (end + start) / 2;
        MergeSort(dict, start, mid);
        MergeSort(dict, mid+1, end);
        Merge(dict, start, mid, end);
    }
    private void Merge(List<CardData> list, int start, int mid, int end)
    {
        int leftLen = mid - start + 1;
        int rightLen = end - mid;
        CardData[] left = new CardData[leftLen];
        CardData[] right= new CardData[rightLen];
        int i;
        int leftInd = 0, rightInd = 0;

        for(i = 0; i < leftLen; i++)
            left[i] = list[start + i];

        for(i = 0; i < rightLen; i++)
            right[i] = list[mid + i + 1];
        
        i = start;

        while(leftInd < leftLen && rightInd < rightLen)
        {
            if(left[leftInd].Card <= right[rightInd].Card)
                list[i++] = left[leftInd++];
            else
                list[i++] = right[rightInd++];
        }

        while(leftInd < leftLen)
            list[i++] = left[leftInd++];

        while(rightInd < rightLen)
            list[i++] = right[rightInd++];
    }

    private int HandType(string value, bool part2)
    {
        int max = 0;
        int max2 = 0;
        int jCount = 0;
        Dictionary<char,int> dict = new();
        for(int i = 0; i < 5; i++)
        {
            if(value[i] == 'J' && part2)
            {
                jCount++;
                continue;
            }
            if(dict.ContainsKey(value[i])) 
            {
                dict[value[i]]++;
                if(dict[value[i]] > max)
                    max = dict[value[i]];
                else if(dict[value[i]] > max2)
                    max2 = dict[value[i]];
                continue;
            }
            if(max == 0)
                max = 1;
            dict[value[i]] = 1;
        }

        max += jCount;
        if(max > 3)
            return max+1;
        else if(max == 3)
            return max + (max2 == 2 ? 1 : 0);
        return max - (max2 == 2 ? 0 : 1);
    }

    private int AssignValues(string card)
    {
        int cardsValue = 0;
        int multi = 1;
        for(int i = card.Length-1; i >= 0; i--)
        {
            cardsValue += values[card[i]] * multi;
            multi *= 15;
        }
        return cardsValue;
    }
}
