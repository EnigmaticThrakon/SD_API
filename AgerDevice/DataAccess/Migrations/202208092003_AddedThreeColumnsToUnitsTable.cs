using FluentMigrator;

namespace AgerDevice.DataAccess.Migrations
{
    [Migration(202208092003)]
    public class AddedThreeColumnsToUnitsTable : Migration
    {
        public override void Up()
        {
            Create.Column("SerialNumber").OnTable("Units").AsString(100).NotNullable();
            Create.Column("IsDeleted").OnTable("Units").AsBoolean().NotNullable().WithDefaultValue(false);
            Create.Column("Modified").OnTable("Units").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Column("SerialNumber").FromTable("Units");
            Delete.Column("IsDeleted").FromTable("Units");
            Delete.Column("Modified").FromTable("Units");
        }
    }
}