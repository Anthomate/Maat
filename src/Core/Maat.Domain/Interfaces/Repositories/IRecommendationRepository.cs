using Maat.Domain.Entities;
using Maat.Domain.Enums;

namespace Maat.Domain.Interfaces.Repositories;

public interface IRecommendationRepository
{
    Task<IEnumerable<Recommendation>> GetByDiagnosticIdAsync(Guid diagnosticId);
    Task<IEnumerable<Recommendation>> GetByCategoryAsync(RseCategoryType category);
    Task<IEnumerable<Recommendation>> GetByStatusAsync(RecommendationStatus status);
    Task<IEnumerable<Recommendation>> GetByPriorityAsync(Priority priority);
}