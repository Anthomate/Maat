using Maat.Application.Features.Auth.Commands.Login;
using Maat.Application.Features.Auth.Commands.Register;
using Maat.Domain.Entities;
using Maat.Domain.Enums;
using Maat.Domain.ValueObjects;

namespace Maat.Api.IntegrationTests.Common;

public static class TestData
{
    public static class Users
    {
        public static User ValidUser => new(
            "test@example.com",
            "hashedPassword123!",
            "John",
            "Doe"
        );

        public static User AdminUser => new(
            "admin@example.com",
            "adminPassword123!",
            "Admin",
            "User"
        );
    }

    public static class Companies
    {
        public static Company ValidCompany => new(
            "Test Company",
            "12345678901234",
            CompanySize.Small,
            IndustryType.Technology,
            new Address("123 Test Street", "Test City", "12345", "Test Country")
        );
    }

    public static class Auth
    {
        public static RegisterCommand ValidRegisterCommand => new()
        {
            Email = "newuser@example.com",
            Password = "Password123!",
            FirstName = "New",
            LastName = "User"
        };

        public static LoginCommand ValidLoginCommand => new()
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
    }
}