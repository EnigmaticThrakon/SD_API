using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208171822)]
    public class AddedIsConnectedToUnitsTable : Migration
    {
        public override void Up()
        {
            Create.Column("IsConnected").OnTable("Units").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IsConnected").FromTable("Units");
        }
    }
}