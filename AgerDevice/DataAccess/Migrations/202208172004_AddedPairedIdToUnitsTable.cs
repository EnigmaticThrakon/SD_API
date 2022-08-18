using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208172004)]
    public class AddedPairedIdToUnitsTable : Migration
    {
        public override void Up()
        {
            Create.Column("PairedId").OnTable("Units").AsGuid().Nullable().WithDefaultValue(null);
        }

        public override void Down()
        {
            Delete.Column("PairedId").FromTable("Units");
        }
    }
}