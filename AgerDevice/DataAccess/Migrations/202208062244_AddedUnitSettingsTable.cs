using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208062244)]
    public class AddedUnitSettingsTable : Migration
    {
        public override void Up()
        {
            Create.Table("UnitSettings")
                .WithColumn("UnitId").AsGuid().PrimaryKey().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UnitSettings");
        }
    }
}