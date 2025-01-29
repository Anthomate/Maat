using Maat.Domain.Entities.Base;

namespace Maat.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Company? Company { get; private set; }
    
    private User() : base() { }

    public User(string email, string passwordHash, string firstName, string lastName) : base()
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdatePersonalInfo(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssociateCompany(Company company)
    {
        company = company;
        UpdatedAt = DateTime.UtcNow;
    }
}