using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal class ExampleUser : IMyEntity<ExampleUser>
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string[] GetColumnsSql()
        {
            return ["Id INTEGER PRIMARY KEY AUTOINCREMENT", "Name TEXT", "Description TEXT"];
        }

        public List<ExampleUser> GetInitializationEntities()
        {
            var initialUsers = new List<ExampleUser>();

            //TODO: Obviously this is not supposed to happen like this, but it is just an example
            initialUsers.Add(new ExampleUser { Name = "Admin", Username = "admin", Password = "admin" });
            return initialUsers;
        }

        public string GetTableName()
        {
            return "User";
        }
    }
}
