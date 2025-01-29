using Maat.Domain.Entities.Base;
using Maat.Domain.Enums;
using Maat.Domain.ValueObjects;

namespace Maat.Domain.Entities;

public class Diagnostic : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; }
    public DiagnosticStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    
    public Score EnvironmentalScore { get; private set; }
    public Score SocialScore { get; private set; }
    public Score EconomicScore { get; private set; }
    public Score GovernanceScore { get; private set; }
    
    private readonly List<QuestionResponse> _responses;
    public IReadOnlyCollection<QuestionResponse> Responses => _responses.AsReadOnly();
    
    private readonly List<Recommendation> _recommendations;
    public IReadOnlyCollection<Recommendation> Recommendations => _recommendations.AsReadOnly();

    private Diagnostic() : base()
    {
        _responses = new List<QuestionResponse>();
        _recommendations = new List<Recommendation>();
        Status = DiagnosticStatus.Started;
    }

    public Diagnostic(Company company) : this()
    {
        Company = company;
        CompanyId = company.Id;

        EnvironmentalScore = new Score(0);
        SocialScore = new Score(0);
        EconomicScore = new Score(0);
        GovernanceScore = new Score(0);
    }

    public void AddResponse(QuestionResponse response)
    {
        var existingResponse = _responses.FirstOrDefault(r => r.QuestionId == response.QuestionId);
        if (existingResponse != null)
        {
            _responses.Remove(existingResponse);
        }
        _responses.Add(response);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void CalculateScores()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = DiagnosticStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRecommendation(Recommendation recommendation)
    {
        _recommendations.Add(recommendation);
        UpdatedAt = DateTime.UtcNow;
    }
}