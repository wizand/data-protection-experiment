using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class InitializeDatabaseWithAccessRights
    {

        DatabaseManager _dbManager;
        public InitializeDatabaseWithAccessRights(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        public int Initialize()
        {
            int inserted = -1;
            AccessRight accessRight = new AccessRight();
            AccessRightAssignment accessRightAssignment = new AccessRightAssignment();
            Application application = new Application();

            if (!_dbManager.CreateTableIfNotExists(accessRight.GetTableName(), accessRight.GetColumnsSql()))
            {
                return inserted;
            }
            if (!_dbManager.CreateTableIfNotExists(accessRightAssignment.GetTableName(), accessRightAssignment.GetColumnsSql()))
            {
                return inserted;
            }
            inserted = 0;
            List<AccessRight> initialAccessRights = accessRight.GetInitializationEntities();
            inserted += _dbManager.InsertAccessRights(initialAccessRights);

            List<AccessRightAssignment> initialAccessRightAssignments = accessRightAssignment.GetInitializationEntities();
            inserted += _dbManager.InsertAccessRightAssignments(initialAccessRightAssignments);

            List<Application> initialApplications = application.GetInitializationEntities();
            inserted += _dbManager.InsertApplications(initialApplications);

            return inserted;
        }


    }
}
