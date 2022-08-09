using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208070041)]
    public class AddedFourColumnsToUsersTable : Migration
    {
        public override void Up()
        {
            Create.Column("SerialNumber").OnTable("Users").AsString(100).NotNullable();
            Create.Column("IsDeleted").OnTable("Users").AsBoolean().NotNullable().WithDefaultValue(false);
            Create.Column("Modified").OnTable("Users").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
            Create.Column("LastConnected").OnTable("Users").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Column("SerialNumber").FromTable("Users");
            Delete.Column("IsDeleted").FromTable("Users");
            Delete.Column("Modified").FromTable("Users");
            Delete.Column("LastConnected").FromTable("Users");
        }
    }
}