using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208092119)]
    public class AddedUserNameToUsers : Migration
    {
        public override void Up()
        {
            Create.Column("UserName").OnTable("Users").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("UserName").FromTable("Users");
        }
    }
}