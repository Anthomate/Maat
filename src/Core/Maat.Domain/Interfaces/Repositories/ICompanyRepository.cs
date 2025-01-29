using Maat.Domain.Entities;
using Maat.Domain.Enums;

namespace Maat.Domain.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetBySiretAsync(string siret);
    Task<bool> ExistsBySiretAsync(string siret);
    Task<IEnumerable<Company>> GetByIndustryTypeAsync(IndustryType industryType);
    Task<IEnumerable<Company>> GetByCompanySizeAsync(CompanySize size);
}