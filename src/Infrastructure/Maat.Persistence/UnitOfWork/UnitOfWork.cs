using Maat.Domain.Interfaces;
using Maat.Domain.Interfaces.Repositories;
using Maat.Persistence.Context;
using Maat.Persistence.Repositories;

namespace Maat.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly MaatDbContext _context;
    private IUserRepository _userRepository;
    private ICompanyRepository _companyRepository;
    private IDiagnosticRepository _diagnosticRepository;
    private IQuestionRepository _questionRepository;
    private IRecommendationRepository _recommendationRepository;

    public UnitOfWork(MaatDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => 
        _userRepository ??= new UserRepository(_context);

    public ICompanyRepository Companies =>
        _companyRepository ??= new CompanyRepository(_context);

    public IDiagnosticRepository Diagnostics =>
        _diagnosticRepository ??= new DiagnosticRepository(_context);

    public IQuestionRepository Questions =>
        _questionRepository ??= new QuestionRepository(_context);

    public IRecommendationRepository Recommendations =>
        _recommendationRepository ??= new RecommendationRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}