using Maat.Domain.Entities.Base;

namespace Maat.Domain.Entities;

public class RecommendationAction : BaseEntity
{
    public string ActionType { get; private set; }
    public string? Description { get; private set; }

    private RecommendationAction() : base() { }

    public RecommendationAction(string actionType, string? description = null) : base()
    {
        ActionType = actionType;
        Description = description;
    }
}