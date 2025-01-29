using Maat.Domain.Entities;
using Maat.Domain.Enums;

namespace Maat.Domain.Interfaces.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByCategoryAsync(RseCategoryType category);
    Task<IEnumerable<Question>> GetByTypeAsync(QuestionType type);
    Task<IEnumerable<Question>> GetOrderedQuestionsAsync();
    Task<int> GetTotalQuestionsCountAsync();
}