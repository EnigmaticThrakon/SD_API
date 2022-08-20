using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208200049)]
    public class AddedThreeColumnsToUserTable : Migration
    {
        public override void Up()
        {
            Create.Column("UserName").OnTable("Users").AsString(100).Nullable();
            Create.Column("GroupId").OnTable("Users").AsGuid().NotNullable();
            Create.Column("GroupsEnabled").OnTable("Users").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("UserName").FromTable("Users");
            Delete.Column("GroupId").FromTable("Users");
            Delete.Column("GroupsEnabled").FromTable("Users");
        }
    }
}