using DataAccess.Entities;


namespace DataAccess
{
    public static class InitializeDatabaseWithAccessRights
    {

        public static int Initialize(DatabaseManager dbManager)
        {
            int inserted = -1;
            //TODO: Make these all as statics
            AccessRight accessRight = new AccessRight();
            AccessRightAssignment accessRightAssignment = new AccessRightAssignment();
            Application application = new Application();
            ExampleUser exampleUser = new ExampleUser();
            if (!dbManager.CreateTableIfNotExists(accessRight.GetTableName(), accessRight.GetColumnsSql()))
            {
                return inserted;
            }
            if (!dbManager.CreateTableIfNotExists(accessRightAssignment.GetTableName(), accessRightAssignment.GetColumnsSql()))
            {
                return inserted;
            }
            if (!dbManager.CreateTableIfNotExists(application.GetTableName(), application.GetColumnsSql()))
            {
                return inserted;
            }
            if (!dbManager.CreateTableIfNotExists(exampleUser.GetTableName(), exampleUser.GetColumnsSql()))
            {
                return inserted;
            }
            inserted = 0;

            int count = dbManager.GetCount(accessRight.GetTableName());
            if (count == 0)
            {
                List<AccessRight> initialAccessRights = accessRight.GetInitializationEntities();
                inserted += dbManager.InsertAccessRights(initialAccessRights);
            }

            count = dbManager.GetCount(application.GetTableName());
            if (count == 0)
            {
                List<Application> initialApplications = application.GetInitializationEntities();
                inserted += dbManager.InsertApplications(initialApplications);
            }

            count = dbManager.GetCount(exampleUser.GetTableName());
            if (count == 0)
            {
                List<ExampleUser> initialUsers = exampleUser.GetInitializationEntities();
                inserted += dbManager.InsertUsers(initialUsers);
            }

            count = dbManager.GetCount(accessRightAssignment.GetTableName());
            if (count == 0)
            {
                List<AccessRightAssignment> initialAccessRightAssignments = accessRightAssignment.GetInitializationEntities();
                inserted += dbManager.InsertAccessRightAssignments(initialAccessRightAssignments);
            }

            return inserted;
        }


    }
}
