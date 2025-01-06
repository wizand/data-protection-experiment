namespace DataAccess.Entities
{
    public class AccessRightAssignment : IMyEntity<AccessRightAssignment>
    {
        public int Id { get; set; }
        public int AccessRightId { get; set; }
        public int UserId { get; set; }
        public int ApplicationId { get; set; }

        public string GetTableName()
        {
            return "AccessRightAssignment";
        }

        public string[] GetColumnsSql()
        {
            return ["Id INTEGER PRIMARY KEY AUTOINCREMENT", "AccessRightId INTEGER", "UserId INTEGER, ApplicationId INTEGER"];
        }

        public List<AccessRightAssignment> GetInitializationEntities()
        {
            return new();
        }

    }
}
