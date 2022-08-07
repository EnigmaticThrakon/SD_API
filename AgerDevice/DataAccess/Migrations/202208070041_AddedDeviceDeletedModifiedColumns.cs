using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208070041)]
    public class AddedDeviceDeletedModifiedColumns : Migration
    {
        public override void Up()
        {
            Create.Column("DeviceId").OnTable("Users").AsString(100).NotNullable();
            Create.Column("IsDeleted").OnTable("Users").AsBoolean().NotNullable().WithDefaultValue(false);
            Create.Column("Modified").OnTable("Users").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("DeviceId").FromTable("Users");
            Delete.Column("IsDeleted").FromTable("Users");
            Delete.Column("Modified").FromTable("Users");
        }
    }
}