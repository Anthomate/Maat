using Maat.Domain.Entities.Base;
using Maat.Domain.Enums;

namespace Maat.Domain.Entities;

public class Recommendation : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public RseCategoryType Category { get; private set; }
    public Priority Priority { get; private set; }
    public RecommendationStatus Status { get; private set; }
    public decimal Impact { get; private set; }
    public string? Notes { get; private set; }
    
    public Guid DiagnosticId { get; private set; }
    public Diagnostic Diagnostic { get; private set; }
    
    private readonly List<RecommendationAction> _actions;
    public IReadOnlyCollection<RecommendationAction> Actions => _actions.AsReadOnly();

    private Recommendation() : base()
    {
        _actions = new List<RecommendationAction>();
        Status = RecommendationStatus.Pending;
    }

    public Recommendation(
        string title,
        string description,
        RseCategoryType category,
        Priority priority,
        decimal impact,
        Diagnostic diagnostic) : this()
    {
        Title = title;
        Description = description;
        Category = category;
        Priority = priority;
        Impact = impact;
        DiagnosticId = diagnostic.Id;
        Diagnostic = diagnostic;
    }
    
    public void MarkAsInProgress(string? notes = null)
    {
        Status = RecommendationStatus.InProgress;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
        _actions.Add(new RecommendationAction("Started implementation", notes));
    }

    public void MarkAsCompleted(string? notes = null)
    {
        Status = RecommendationStatus.Completed;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
        _actions.Add(new RecommendationAction("Completed implementation", notes));
    }

    public void MarkAsRejected(string reason)
    {
        Status = RecommendationStatus.Rejected;
        Notes = reason;
        UpdatedAt = DateTime.UtcNow;
        _actions.Add(new RecommendationAction("Rejected", reason));
    }

    public void AddNote(string note)
    {
        Notes = note;
        UpdatedAt = DateTime.UtcNow;
        _actions.Add(new RecommendationAction("Note added", note));
    }
}