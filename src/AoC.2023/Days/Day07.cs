using System.Diagnostics;
using AoC.Shared;

namespace AoC._2023.Days;

public class Day07 : DayBase<List<Day07.Hand>, long>
{
    protected override string Name { get; }
    
    protected override List<Hand> ParseInput(IEnumerable<string> lines)
    {
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var cards = parts[0].Select(Card.FromSymbol).ToList();
            var bid = long.Parse(parts[1]);
            
            hands.Add(new Hand(cards, bid));
        }

        return hands;
    }

    protected override long RunPart1(List<Hand> input)
    {
        return input.Order().Select((t, i) => (i + 1) * t.Bid).Sum();
    }

    protected override long RunPart2(List<Hand> input)
    {
        return 0;
    }

    public class Hand : IComparable<Hand>
    {
        public List<Card> Cards { get; }
        public long Bid { get; }
        public HandResult Result { get; }

        public Hand(List<Card> cards, long bid)
        {
            Cards = cards;
            Bid = bid;
            Result = HandResult.GetHighestResult(cards);
        }

        public int CompareTo(Hand? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            
            var result = Result.CompareTo(other.Result);
            if (result != 0)
                return result;

            for (var i = 0; i < Cards.Count; i++)
            {
                result = other.Cards[i].CompareTo(Cards[i]);
                if (result != 0)
                    return result;
            }

            return 0;
        }
    }

    public class Card : IComparable<Card>, IEquatable<Card>
    {
        public static readonly Card Ace = new("Ace", 'A', 14);
        public static readonly Card King = new("King", 'K', 13);
        public static readonly Card Queen = new("Queen", 'Q', 12);
        public static readonly Card Jack = new("Jack", 'J', 11);
        public static readonly Card Ten = new("10", 'T', 10);
        public static readonly Card Nine = new("9", '9', 9);
        public static readonly Card Eight = new("8", '8', 8);
        public static readonly Card Seven = new("7", '7', 7);
        public static readonly Card Six = new("6", '6', 6);
        public static readonly Card Five = new("5", '5', 5);
        public static readonly Card Four = new("4", '4', 4);
        public static readonly Card Three = new("3", '3', 3);
        public static readonly Card Two = new("2", '2', 2);
        
        public string Name { get; }
        public char Symbol { get; }
        public long Points { get; }
        
        private Card(string name, char symbol, long points)
        {
            Name = name;
            Symbol = symbol;
            Points = points;
        }

        public static Card FromSymbol(char symbol)
        {
            return symbol switch
            {
                'A' => Ace,
                'K' => King,
                'Q' => Queen,
                'J' => Jack,
                'T' => Ten,
                '9' => Nine,
                '8' => Eight,
                '7' => Seven,
                '6' => Six,
                '5' => Five,
                '4' => Four,
                '3' => Three,
                '2' => Two,
                _ => throw new UnreachableException()
            };
        }

        public int CompareTo(Card? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return other.Points.CompareTo(Points);
        }

        public bool Equals(Card? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Points == other.Points;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Card) obj);
        }

        public override int GetHashCode()
        {
            return Points.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} [{Symbol}] ({Points})";
        }
    }

    public abstract class HandResult : IComparable<HandResult>
    {
        public abstract int Score { get; }

        public static HandResult GetHighestResult(List<Card> cards)
        {
            var groups = cards
                .GroupBy(c => c.Symbol)
                .OrderByDescending(g => g.Count())
                .ToList();
            
            if (groups[0].Count() == 5)
                return new FiveOfAKind();

            if (groups[0].Count() == 4)
                return new FourOfAKind();

            if (groups[0].Count() == 3 && groups[1].Count() == 2)
                return new FullHouse();

            if (groups[0].Count() == 3)
                return new ThreeOfAKind();

            if (groups[0].Count() == 2 && groups[1].Count() == 2)
                return new TwoPair();

            if (groups[0].Count() == 2)
                return new OnePair();

            return new HighCard();
        }

        public int CompareTo(HandResult? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Score.CompareTo(other.Score);
        }
    }

    public class FiveOfAKind : HandResult
    {
        public override int Score => 7;
    }

    public class FourOfAKind : HandResult
    {
        public override int Score => 6;
    }

    public class FullHouse : HandResult
    {
        public override int Score => 5;
    }

    public class ThreeOfAKind : HandResult
    {
        public override int Score => 4;
    }

    public class TwoPair : HandResult
    {
        public override int Score => 3;
    }

    public class OnePair : HandResult
    {
        public override int Score => 2;
    }

    public class HighCard : HandResult
    {
        public override int Score => 1;
    }
}
