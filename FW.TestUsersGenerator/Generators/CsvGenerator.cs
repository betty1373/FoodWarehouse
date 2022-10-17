using FW.TestUsersGenerator.Models;
using ServiceStack.Text;

namespace FW.TestUsersGenerator.Generators
{
    public class CsvGenerator
    {
        public void Generate(string fileName, IEnumerable<UserData> users)
        {
            using var stream = File.Create(fileName);
            CsvSerializer.SerializeToStream(users, stream);
        }
    }
}