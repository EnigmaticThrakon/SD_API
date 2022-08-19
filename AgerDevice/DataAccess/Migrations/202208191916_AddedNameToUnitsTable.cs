using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208191916)]
    public class AddedNameToUnitsTable : Migration
    {
        public override void Up()
        {
            Create.Column("Name").OnTable("Units").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Name").FromTable("Units");
        }
    }
}