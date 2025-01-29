using Maat.Domain.Entities;
using Maat.Domain.Enums;
using Maat.Domain.Interfaces.Repositories;
using Maat.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Maat.Persistence.Repositories;

public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(MaatDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Question>> GetByCategoryAsync(RseCategoryType category)
    {
        return await _dbSet
            .Include(q => q.Options)
            .Where(q => q.Category == category)
            .OrderBy(q => q.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByTypeAsync(QuestionType type)
    {
        return await _dbSet
            .Include(q => q.Options)
            .Where(q => q.Type == type)
            .OrderBy(q => q.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetOrderedQuestionsAsync()
    {
        return await _dbSet
            .Include(q => q.Options)
            .OrderBy(q => q.Category)
            .ThenBy(q => q.Order)
            .ToListAsync();
    }

    public async Task<int> GetTotalQuestionsCountAsync()
    {
        return await _dbSet.CountAsync();
    }
}