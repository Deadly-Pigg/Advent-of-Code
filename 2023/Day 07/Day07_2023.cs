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
    Dictionary<char, int> cardValues = new Dictionary<char, int>()
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
        long sum = 0;
        int rank = 1;

        string[] file = File.ReadAllLines("AoC7.txt"); 
        
        if(part2)
            cardValues['J'] = 1; //part 2 states that the value of the 'Joker' card should be the lowest

        if(handTypes.Count != 0)
            handTypes.Clear(); //need to ensure the dictionary is clear since both parts are executed in conjunction

        Parse(file, part2); //for storing and sorting the decks' value and worth

        for(int i = 0; i < 7; i++) { //goes from lowest rank to highest, adding their strength to the sum
            List<CardData> dict = handTypes[i];
            foreach(CardData value in dict)
            {
                sum += rank * value.Worth;
                rank++;
            }
        }

        if(part2)
            handTypes.Clear();

        return sum;
    }

    private void Parse(string[] file, bool part2) 
    {
        List<CardData>[] decks = new List<CardData>[7]; //an array of list. each list stores deck data, but they are split into separate lists for the types of hands.

        for(int i = 0; i < 7; i++)
            decks[i] = new List<CardData>();

        for(int i = 0; i < file.Length; i++) //reads the input and stores it in the lists (base-15 card values + their worth)
        {
            string[] input = file[i].Split(" ");
            int hand = HandType(input[0], part2);
            decks[hand].Add(new CardData(int.Parse(input[1]), AssignValues(input[0])));
        }

        for(int i = 0; i < 7; i++) //sorting the cards in an appropriate order
        {
            MergeSort(decks[i], 0, decks[i].Count-1);
            handTypes.Add(i, decks[i]);
        }
    }

    private int HandType(string value, bool part2) //Basically counts each type of card and the amount of them, returning the count of the majority card.
    { //also stores the 2nd majority, but that's just for 2 pair/full house.
        int max = 0;
        int secondMax = 0;
        int jokers = 0;

        Dictionary<char,int> dict = new();

        for(int i = 0; i < 5; i++)
        {
            if(value[i] == 'J' && part2) //since joker cards can be anything, we store them seperately.
            {
                jokers++;
                continue;
            }
            if(dict.ContainsKey(value[i]))
            {
                dict[value[i]]++;
                if(dict[value[i]] > max)
                    max = dict[value[i]];
                else if(dict[value[i]] > secondMax)
                    secondMax = dict[value[i]];
                continue;
            }
            if(max == 0)
                max = 1;
            dict[value[i]] = 1;
        }

        max += jokers;
        if(max > 3) //4/5 of a kind
            return max+1;
        else if(max == 3) //3 of a kind / full house
            return max + (secondMax == 2 ? 1 : 0);
        return max - (secondMax == 2 ? 0 : 1); //2 pair / pair / highest card
    }

    private int AssignValues(string deck) //basically converts the cards from a weird base-15 to denary so that sorting is easier (we need to sort to get the lowest label -> highest anyway)
    {
        int cardsValue = 0;
        int multi = 1;
        for(int i = deck.Length-1; i >= 0; i--)
        {
            cardsValue += cardValues[deck[i]] * multi;
            multi *= 15;
        }
        return cardsValue;
    }



    private void MergeSort(List<CardData> dict, int start, int end)//sorting algo. Kinda copy-pasted tbf, but I added changes because certain parts of the merge-sort algo provided on google made no sense.
    { //I'd suggest reading off google for merge sort.
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

        int i, leftInd = 0, rightInd = 0;

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
}
