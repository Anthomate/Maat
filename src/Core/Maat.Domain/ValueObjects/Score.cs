namespace Maat.Domain.ValueObjects;

public class Score
{
    public decimal Value { get; private set; }
    private Score() { }

    public Score(decimal value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Score must be between 0 and 100");
        
        Value = value;
    }
    
    public static Score FromPercentage(decimal percentage) => new(Math.Min(100, Math.Max(0, percentage)));

    public override string ToString() => $"{Value:F1}";
}