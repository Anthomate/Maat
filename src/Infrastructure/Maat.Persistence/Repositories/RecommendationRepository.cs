using Maat.Domain.Entities;
using Maat.Domain.Enums;
using Maat.Domain.Interfaces.Repositories;
using Maat.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Maat.Persistence.Repositories;

public class RecommendationRepository : BaseRepository<Recommendation>, IRecommendationRepository
{
    public RecommendationRepository(MaatDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Recommendation>> GetByDiagnosticIdAsync(Guid diagnosticId)
    {
        return await _dbSet
            .Include(r => r.Actions)
            .Where(r => r.DiagnosticId == diagnosticId)
            .OrderByDescending(r => r.Priority)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByCategoryAsync(RseCategoryType category)
    {
        return await _dbSet
            .Include(r => r.Actions)
            .Where(r => r.Category == category)
            .OrderByDescending(r => r.Priority)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByStatusAsync(RecommendationStatus status)
    {
        return await _dbSet
            .Include(r => r.Actions)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.Priority)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByPriorityAsync(Priority priority)
    {
        return await _dbSet
            .Include(r => r.Actions)
            .Where(r => r.Priority == priority)
            .ToListAsync();
    }
}