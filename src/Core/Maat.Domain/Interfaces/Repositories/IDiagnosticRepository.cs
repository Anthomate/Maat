using Maat.Domain.Entities;
using Maat.Domain.Enums;

namespace Maat.Domain.Interfaces.Repositories;

public interface IDiagnosticRepository
{
    Task<IEnumerable<Diagnostic>> GetByCompanyIdAsync(Guid companyId);
    Task<Diagnostic?> GetLatestByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<Diagnostic>> GetByStatusAsync(DiagnosticStatus status);
    Task<IEnumerable<Diagnostic>> GetCompletedBetweenDatesAsync(DateTime start, DateTime end);
}