using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208122132)]
    public class AddedFourColumnsToUserSettings : Migration
    {
        public override void Up()
        {
            Create.Column("UserName").OnTable("UserSettings").AsString(100).Nullable();
            Create.Column("Modified").OnTable("UserSettings").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
            Create.Column("GroupId").OnTable("UserSettings").AsGuid().NotNullable();
            Create.Column("GroupsEnabled").OnTable("UserSettings").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("UserName").FromTable("UserSettings");
            Delete.Column("Modified").FromTable("UserSettings");
            Delete.Column("GroupId").FromTable("UserSettings");
            Delete.Column("GroupsEnabled").FromTable("UserSettings");
        }
    }
}