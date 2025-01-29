using Maat.Domain.Entities.Base;
using Maat.Domain.Enums;
using Maat.Domain.ValueObjects;

namespace Maat.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Siret { get; private set; }
    public CompanySize Size { get; private set; }
    public IndustryType Industry { get; private set; }
    public string? Website { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Address Address { get; private set; }
    
    public ICollection<User> Users { get; private set; }
    public ICollection<Diagnostic> Diagnostics { get; private set; }

    private Company() : base()
    {
        Users = new List<User>();
        Diagnostics = new List<Diagnostic>();
    }

    public Company(
        string name,
        string siret,
        CompanySize size,
        IndustryType industry,
        Address address) : base()
    {
        Name = name;
        Siret = siret;
        Size = size;
        Industry = industry;
        Address = address;
        Users = new List<User>();
        Diagnostics = new List<Diagnostic>();
    }

    public void UpdateBasicInfo(
        string name,
        string? description,
        CompanySize size,
        IndustryType industry)
    {
        Name = name;
        Description = description;
        Size = size;
        Industry = industry;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContact(
        string? website,
        string? phoneNumber,
        Address address)
    {
        Website = website;
        PhoneNumber = phoneNumber;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddUser(User user)
    {
        Users.Add(user);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddDiagnostic(Diagnostic diagnostic)
    {
        Diagnostics.Add(diagnostic);
        UpdatedAt = DateTime.UtcNow;
    }
}