using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062243)]
    public class AddedUserSettingsTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserSettings")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UserSettings");
        }
    }
}