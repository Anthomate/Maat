using Maat.Domain.Entities;
using Maat.Domain.Enums;
using Maat.Domain.Interfaces.Repositories;
using Maat.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Maat.Persistence.Repositories;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(MaatDbContext context) : base(context)
    {
    }

    public async Task<Company?> GetBySiretAsync(string siret)
    {
        return await _dbSet
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Siret == siret);
    }

    public async Task<bool> ExistsBySiretAsync(string siret)
    {
        return await _dbSet.AnyAsync(c => c.Siret == siret);
    }

    public async Task<IEnumerable<Company>> GetByIndustryTypeAsync(IndustryType industryType)
    {
        return await _dbSet
            .Where(c => c.Industry == industryType)
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetByCompanySizeAsync(CompanySize size)
    {
        return await _dbSet
            .Where(c => c.Size == size)
            .ToListAsync();
    }
}