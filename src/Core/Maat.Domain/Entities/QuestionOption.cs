namespace Maat.Domain.Entities;

public class QuestionOption
{
    public string Text { get; private set; }
    public decimal Score { get; private set; }

    private QuestionOption() : base() { }

    public QuestionOption(string text, decimal score) : base()
    {
        Text = text;
        Score = score;
    }
}