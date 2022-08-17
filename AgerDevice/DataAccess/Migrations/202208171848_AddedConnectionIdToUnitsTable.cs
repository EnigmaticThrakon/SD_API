using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208171848)]
    public class AddedConnectionIdToUnitsTable : Migration
    {
        public override void Up()
        {
            Create.Column("ConnectionId").OnTable("Units").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ConnectionId").FromTable("Units");
        }
    }
}