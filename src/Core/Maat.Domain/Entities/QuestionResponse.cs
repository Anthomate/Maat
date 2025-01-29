using Maat.Domain.Entities.Base;

namespace Maat.Domain.Entities;

public class QuestionResponse : BaseEntity
{
    public Guid DiagnosticId { get; private set; }
    public Guid QuestionId { get; private set; }
    public string Response { get; private set; }
    public decimal Score { get; private set; }
    
    private QuestionResponse() { }

    public QuestionResponse(Guid diagnosticId, Guid questionId, string response, decimal score) : base()
    {
        DiagnosticId = diagnosticId;
        QuestionId = questionId;
        Response = response;
        Score = score;
    }
}