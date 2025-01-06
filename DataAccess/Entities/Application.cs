using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Application : IMyEntity<Application>
    {


        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string[] GetColumnsSql()
        {
            return ["Id INTEGER PRIMARY KEY AUTOINCREMENT", "Name TEXT", "Description TEXT"];
        }

        public List<Application> GetInitializationEntities()
        {
            List<Application> initialApplications = new List<Application>();
            initialApplications.Add(new Application { Name = "DataProtectionApi", Description = "The api application" });
            initialApplications.Add(new Application { Name = "Blazor example", Description = "The web ui example application" });
            return initialApplications;
        }

        public string GetTableName()
        {
            return "Application";
        }
    }
}
