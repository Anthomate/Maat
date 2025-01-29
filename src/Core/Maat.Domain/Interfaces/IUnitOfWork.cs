using Maat.Domain.Interfaces.Repositories;

namespace Maat.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICompanyRepository Companies { get; }
    IDiagnosticRepository Diagnostics { get; }
    IQuestionRepository Questions { get; }
    IRecommendationRepository Recommendations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}