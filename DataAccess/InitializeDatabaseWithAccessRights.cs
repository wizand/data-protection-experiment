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
                var usersFromDb = dbManager.QueryAllFrom<ExampleUser>(exampleUser.GetTableName());
                var applicationsFromDb = dbManager.QueryAllFrom<Application>(application.GetTableName());
                var accessRightsFromDb = dbManager.QueryAllFrom<AccessRight>(accessRight.GetTableName());

                ExampleUser? adminUser = null;
                for (int i = 0; i < usersFromDb.Length; i++)
                {
                    if (usersFromDb[i].Name == "Admin")
                    {
                        adminUser = usersFromDb[i];
                        break;
                    }
                }

                AccessRight? adminRight = null;
                for (int i = 0; i < accessRightsFromDb.Length; i++)
                {
                    if (accessRightsFromDb[i].Name == "Admin")
                    {
                        adminRight = accessRightsFromDb[i];
                        break;
                    }
                }

                if (adminRight == null || adminUser == null)
                {
                    throw new Exception("No admin right or user in database.");
                }

                foreach (var currentApplication in applicationsFromDb)
                {
                    AccessRightAssignment assignmentForAdmin = new();
                    assignmentForAdmin.AccessRightId = adminRight.Id;
                    assignmentForAdmin.ApplicationId = currentApplication.Id;
                    assignmentForAdmin.UserId = adminUser.Id;
                    initialAccessRightAssignments.Add(assignmentForAdmin);
                }

                inserted += dbManager.InsertAccessRightAssignments(initialAccessRightAssignments);
            }

            return inserted;
        }


    }
}
