namespace DataAccess.Entities
{
    public class AccessRight : IMyEntity<AccessRight>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string GetTableName()
        {
            return "AccessRight";
        }
        public string[] GetColumnsSql()
        {
            return ["Id INTEGER PRIMARY KEY AUTOINCREMENT", "Name TEXT", "Description TEXT"];
        }

        public List<AccessRight> GetInitializationEntities()
        {
            List<AccessRight> initialAccessRights = new List<AccessRight>();

            initialAccessRights.Add(new AccessRight { Name = "Admin", Description = "Admin Access" });
            initialAccessRights.Add(new AccessRight { Name = "ReadOnly", Description = "Read only access" });
            initialAccessRights.Add(new AccessRight { Name = "ReadWrite", Description = "Read and write access" });

            return initialAccessRights;
        }

    }
}
