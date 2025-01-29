using Maat.Domain.Entities;
using Maat.Domain.Enums;
using Maat.Domain.Interfaces.Repositories;
using Maat.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Maat.Persistence.Repositories;

public class DiagnosticRepository : BaseRepository<Diagnostic>, IDiagnosticRepository
{
    public DiagnosticRepository(MaatDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Diagnostic>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _dbSet
            .Include(d => d.Responses)
            .Include(d => d.Recommendations)
            .Where(d => d.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<Diagnostic?> GetLatestByCompanyIdAsync(Guid companyId)
    {
        return await _dbSet
            .Include(d => d.Responses)
            .Include(d => d.Recommendations)
            .Where(d => d.CompanyId == companyId)
            .OrderByDescending(d => d.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Diagnostic>> GetByStatusAsync(DiagnosticStatus status)
    {
        return await _dbSet
            .Include(d => d.Company)
            .Where(d => d.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Diagnostic>> GetCompletedBetweenDatesAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(d => d.Company)
            .Where(d => d.Status == DiagnosticStatus.Completed
                        && d.CompletedAt >= start 
                        && d.CompletedAt <= end)
            .ToListAsync();
    }
}