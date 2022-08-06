using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208061340)]
    public class CreatingInitialTables : Migration
    {
        public override void Up()
        {
            Create.Table("Users").WithAgerDeviceModel();
            Create.Table("Units").WithAgerDeviceModel();
            Create.Table("Groups").WithAgerDeviceModel();
            Create.Table("UserSettings").WithAgerDeviceModel();
            Create.Table("UnitSettings").WithAgerDeviceModel();
        }

        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("Units");
            Delete.Table("Groups");
            Delete.Table("UserSettings");
            Delete.Table("UnitSettings");
        }
    }
}