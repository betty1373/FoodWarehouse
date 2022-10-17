using Bogus;
using FW.TestUsersGenerator.Models;

namespace FW.TestUsersGenerator.Generators
{
    public static class RandomUserGenerator
    {
        public static UserData Generate()
        {
            var userFaker = CreateFaker();
            return userFaker.Generate();
        }

        private static Faker<UserData> CreateFaker()
        {
            return new Faker<UserData>()
                .CustomInstantiator(f => new UserData())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
                .RuleFor(u => u.Password, f => f.Internet.Password(10))
                .RuleFor(u => u.WarehouseName, f => f.Company.CompanyName())
                .RuleFor(u => u.WarehouseAddress, f => f.Address.FullAddress());
        }
    }
}