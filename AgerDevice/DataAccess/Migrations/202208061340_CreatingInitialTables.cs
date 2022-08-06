using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208061340)]
    public class CreatingInitialTables : Migration
    {
        public override void Up()
        {
            Create.Table("Users");
            Create.Table("Units");
            Create.Table("Groups");
            Create.Table("UserSettings");
            Create.Table("UnitSettings");
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