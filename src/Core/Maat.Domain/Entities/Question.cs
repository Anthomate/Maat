using Maat.Domain.Entities.Base;
using Maat.Domain.Enums;

namespace Maat.Domain.Entities;

public class Question : BaseEntity
{
    public string Text { get; private set; }
    public string? Description { get; private set; }
    public QuestionType Type { get; private set; }
    public RseCategoryType Category { get; private set; }
    public int Order { get; private set; }
    public bool IsRequired { get; private set; }
    public decimal Weight { get; private set; }
    
    private readonly List<QuestionOption> _options;
    public IReadOnlyCollection<QuestionOption> Options => _options.AsReadOnly();

    private Question() : base()
    {
        _options = new List<QuestionOption>();
    }
    
    public Question(
        string text,
        QuestionType type,
        RseCategoryType category,
        int order,
        bool isRequired = true,
        decimal weight = 1.0m,
        string? description = null) : base()
    {
        Text = text;
        Type = type;
        Category = category;
        Order = order;
        IsRequired = isRequired;
        Weight = weight;
        Description = description;
        _options = new List<QuestionOption>();
    }
    
    public void AddOption(string text, decimal score)
    {
        if (Type != QuestionType.SingleChoice && Type != QuestionType.MultipleChoice)
            throw new InvalidOperationException("Can't add options to non-choice questions");

        _options.Add(new QuestionOption(text, score));
        UpdatedAt = DateTime.UtcNow;
    }
    
    public decimal CalculateScore(string response)
    {
        return Type switch
        {
            QuestionType.YesNo => CalculateYesNoScore(response),
            QuestionType.SingleChoice => CalculateSingleChoiceScore(response),
            QuestionType.MultipleChoice => CalculateMultipleChoiceScore(response),
            QuestionType.Scale => CalculateScaleScore(response),
            _ => throw new NotImplementedException($"Score calculation not implemented for {Type}")
        };
    }

    private decimal CalculateYesNoScore(string response)
    {
        return response.ToLower() == "yes" ? 100 : 0;
    }

    private decimal CalculateSingleChoiceScore(string response)
    {
        var option = _options.FirstOrDefault(o => o.Text == response);
        return option?.Score ?? 0;
    }

    private decimal CalculateMultipleChoiceScore(string response)
    {
        var selectedOptions = response.Split(',');
        var totalScore = _options
            .Where(o => selectedOptions.Contains(o.Text))
            .Average(o => o.Score);
        return totalScore;
    }

    private decimal CalculateScaleScore(string response)
    {
        if (int.TryParse(response, out int value))
        {
            return (value / 5m) * 100;
        }
        return 0;
    }
}