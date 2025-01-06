using System.Text;

using Dapper;

using DataAccess.Entities;

using Microsoft.Data.Sqlite;

namespace DataAccess
{
    public class DatabaseManager
    {

        private readonly string _connectionString;
        private const string _createDatabase = "CREATE DATABASE IF NOT EXISTS AccessRights ();";

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }


        public bool CreateTableIfNotExists(string tableName, string[] columns)
        {
            int numOfRowsAffected = 0;
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder("CREATE DATABASE IF NOT EXISTS ");
                sb.Append(tableName);
                sb.Append(" (");
                for (int i = 0; i < columns.Length; i++)
                {
                    sb.Append(columns[i]);
                    if (i < columns.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(");");

                numOfRowsAffected = connection.Execute(sb.ToString());

            }


            if (numOfRowsAffected == 0)
            {
                return false;
            }

            return true;
        }

        internal int Insert<T>(string insertStatement, List<T> entities)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    return connection.Execute(insertStatement, entities, transaction: transaction);
                }
            }
        }

        internal int InsertAccessRights(List<AccessRight> accessRights)
        {
            string insertStatement = "INSERT INTO AccessRight (Name, Description) VALUES (@Name, @Description)";
            return Insert<AccessRight>(insertStatement, accessRights);
        }

        internal int InsertAccessRightAssignments(List<AccessRightAssignment> accessRightAssignments)
        {
            string insertStatement = "INSERT INTO AccessRightAssignment (AccessRightId, UserId, ApplicationId) VALUES (@AccessRightId, @UserId, @ApplicationId)";
            return Insert<AccessRightAssignment>(insertStatement, accessRightAssignments);
        }

        internal int InsertApplications(List<Application> applications)
        {
            string insertStatement = "INSERT INTO Application (Name, Description) VALUES (@Name, @Description)";
            return Insert<Application>(insertStatement, applications);
        }
    }
}
